using MathNet.Numerics.Distributions;

using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Engine.Analysis.Utils {

	public enum MonteCarloRV {
		Normal,
		LogNormal,
		//The following are currently not implemented, but are supported by Math.NET
		ContinuousUniform,
		Beta,
		Cauchy,
		Chi,
		ChiSquared,
		Erlang,
		Exponential,
		FisherSnedecor,
		Gamma,
		InverseGamma,
		Laplace,
		Pareto,
		Rayleigh,
		Stable,
		StudentT,
		Weibull,
		Triangular,
	}


	public class MonteCarlo {
		//internal IContinuousDistribution RandomVariable { get; init; }
		internal decimal BasePrice { get; init; }
		internal int AnalysisLength { get; init; }
		internal int SimulationCount { get; init; }
		internal decimal RandomVarScaleFactor { get; init; }
		internal OptionsDict RandomVariableOptions { get; init; }

		public MonteCarlo(OptionsDict options) {
			BasePrice = decimal.Parse(options["basePrice"]);
			AnalysisLength = int.Parse(options["analysisLength"]);
			RandomVarScaleFactor = decimal.Parse(options["randomVariableScaleFactor"]);
			SimulationCount = int.Parse(options["simCount"]);
			//RandomVariable = randomVariable;

			RandomVariableOptions = new OptionsDict() {
				["randomVariableType"] = options["randomVariableType"],
				["randomVariableMu"] = options["randomVariableMu"],
				["randomVariableSigma"] = options["randomVariableSigma"],
			};

			RandomVariableOptions.Remove("basePrice");
			RandomVariableOptions.Remove("analysisLength");
			RandomVariableOptions.Remove("randomVariableScaleFactor");
			RandomVariableOptions.Remove("simCount");
		}

		public static IContinuousDistribution CreateRandomVariable(OptionsDict parameters) {
			var rvType = Enum.Parse<MonteCarloRV>(parameters["randomVariableType"]);

			return rvType switch {
				MonteCarloRV.Normal => new Normal(double.Parse(parameters["randomVariableMu"]), double.Parse(parameters["randomVariableSigma"])),
				MonteCarloRV.LogNormal => new LogNormal(double.Parse(parameters["randomVariableMu"]), double.Parse(parameters["randomVariableSigma"])),
				_ => throw new NotImplementedException(),
			};
		}

		internal virtual void SingleIteration(IContinuousDistribution rv, ref List<decimal> outModel) {
			var currentPrice = BasePrice;
			for (var step = 0; step < AnalysisLength; step++) {
				outModel.Add(currentPrice);
				currentPrice += RandomVarScaleFactor
								* (decimal)rv.Sample();
			}
		}

		public InvestmentModel RunSimulation() {
			var finalModel = new InvestmentModel();

			var threads = Environment.ProcessorCount * 2;
			var subIterations = (int)Math.Ceiling((double)SimulationCount / threads);
			var mergedIterations = 0;
			var tasks = new Task[threads];
			for (int i = 0; i < tasks.Length; i++) {
				tasks[i] = new Task(() => {
					var subModel = new InvestmentModel();
					var rv = CreateRandomVariable(RandomVariableOptions);

					var iterResults = new List<decimal>();
					for (int iter = 0; iter < subIterations; iter++) {
						SingleIteration(rv, ref iterResults);
						if (iter == 0) {
							subModel.MinModelData = new List<decimal>(iterResults);
							subModel.MaxModelData = new List<decimal>(iterResults);
							subModel.AvgModelData = new List<decimal>(iterResults);
							continue;
						} else {
							for (int step = 0; step < AnalysisLength; step++) {
								subModel.MinModelData[step] = Math.Min(subModel.MinModelData[step], iterResults[step]);
								subModel.MaxModelData[step] = Math.Max(subModel.MaxModelData[step], iterResults[step]);
								subModel.AvgModelData[step] = subModel.AvgModelData[step]
															+ (iterResults[step] - subModel.AvgModelData[step]) / (iter + 1); //Cumulative Moving Average
							}
						}
						iterResults.Clear();
					}

					lock (finalModel) {
						if (finalModel.MinModelData.Count == 0) {
							finalModel.MinModelData = new List<decimal>(subModel.MinModelData);
							finalModel.MaxModelData = new List<decimal>(subModel.MaxModelData);
							finalModel.AvgModelData = new List<decimal>(subModel.AvgModelData);
						} else {
							finalModel.MinModelData = finalModel.MinModelData.Zip(subModel.MinModelData, (x, y) => Math.Min(x, y)).ToList();
							finalModel.MaxModelData = finalModel.MaxModelData.Zip(subModel.MaxModelData, (x, y) => Math.Max(x, y)).ToList();
							finalModel.AvgModelData = finalModel.AvgModelData.Zip(subModel.AvgModelData, (x, y) => x + (y - x) / (mergedIterations + 1)).ToList();
						}

						Interlocked.Increment(ref mergedIterations);
					}
				});
				tasks[i].Start();
			}

			try {
				Task.WaitAll(tasks);
			} catch (AggregateException e) {
				Console.WriteLine("Exception %s Thrown in subtask e");
				foreach (var v in e.InnerExceptions) {
					Console.WriteLine(e.Message + " " + v.Message);
				}
			} finally {
				foreach (var task in tasks) {
					task.Dispose();
				}
			}
			//Force GC of Simulation Assets
			GC.Collect(2);
			return finalModel;
		}

	}
}
