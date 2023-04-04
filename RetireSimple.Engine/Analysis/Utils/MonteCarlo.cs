using MathNet.Numerics.Distributions;

using Microsoft.Extensions.Options;

using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.Data.Investment;

using System.Collections.Concurrent;

namespace RetireSimple.Engine.Analysis.Utils {

	internal interface IMonteCarlo {
		//IContinuousDistribution CreateRandomVariable(MonteCarloRV rvType, OptionsDict parameters);
		List<decimal> MonteCarloSingleSimulation();
		InvestmentModel RunSimulation();
		InvestmentModel FilterSimulationData(ConcurrentBag<List<decimal>> simLists, int analysisLength);

	}

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


	public class MonteCarlo : IMonteCarlo {
		internal IContinuousDistribution RandomVariable { get; init; }
		internal decimal BasePrice { get; init; }
		internal int AnalysisLength { get; init; }
		internal int SimulationCount { get; init; }
		internal decimal RandomVarScaleFactor { get; init; }

		public MonteCarlo(OptionsDict options, IContinuousDistribution randomVariable) {
			BasePrice = decimal.Parse(options["basePrice"]);
			AnalysisLength = int.Parse(options["analysisLength"]);
			RandomVarScaleFactor = decimal.Parse(options["randomVariableScaleFactor"]);
			SimulationCount = int.Parse(options["simCount"]);
			RandomVariable = randomVariable;
		}

		public static IContinuousDistribution CreateRandomVariable(OptionsDict parameters) {
			var rvType = Enum.Parse<MonteCarloRV>(parameters["randomVariableType"]);
			
			return rvType switch {
				MonteCarloRV.Normal => new Normal(double.Parse(parameters["randomVariableMu"]), double.Parse(parameters["randomVariableSigma"])),
				MonteCarloRV.LogNormal => new LogNormal(double.Parse(parameters["randomVariableMu"]), double.Parse(parameters["randomVariableSigma"])),
				_ => throw new NotImplementedException(),
			};
		}

		public List<decimal> MonteCarloSingleSimulation() {
			var currentPrice = BasePrice;
			var iterModel = new List<decimal>();

			for (var step = 0; step < AnalysisLength; step++) {
				iterModel.Add(currentPrice);
				currentPrice += RandomVarScaleFactor
								* (decimal)RandomVariable.Sample();
			}

			return iterModel;
		}

		public InvestmentModel RunSimulation() {
			var simLists = new ConcurrentBag<List<decimal>>();
			Parallel.For(0, SimulationCount, x => {
				simLists.Add(MonteCarloSingleSimulation());
			});

			return FilterSimulationData(simLists, AnalysisLength);
		}

		public InvestmentModel FilterSimulationData(ConcurrentBag<List<decimal>> simLists, int analysisLength) {
			var model = new InvestmentModel();
			for (var i = 0; i < analysisLength; i++) {
				model.MinModelData.Add(simLists.Select(x => x[i]).Min());
				model.MaxModelData.Add(simLists.Select(x => x[i]).Max());
				model.AvgModelData.Add(simLists.Select(x => x[i]).Average());
			}

			return model;
		}

	}
}
