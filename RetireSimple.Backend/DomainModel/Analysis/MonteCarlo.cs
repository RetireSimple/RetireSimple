using MathNet.Numerics.Distributions;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using System.Collections.Concurrent;

namespace RetireSimple.Backend.DomainModel.Analysis {

	public enum MonteCarloRV {
		NORMAL,
		LOGNORMAL,
		//The following are currently not implemented, but are supported by Math.NET
		CONTINUOUS_UNIF,
		BETA,
		CAUCHY,
		CHI,
		CHI_SQ,
		ERLANG,
		EXPONENTIAL,
		FISHER_SNEDECOR,
		GAMMA,
		GAMMA_INV,
		LAPLACE,
		PARETO,
		RAYLEIGH,
		STABLE,
		STUDENT_T,
		WEIBULL,
		TRIANGULAR
	}

	public class MonteCarlo {

		/// <summary>
		/// Options Data Structure for Monte Carlo Simulation Parameters
		/// </summary>
		public record struct MonteCarloOptions {
			public decimal BasePrice { get; init; }
			public int AnalysisLength { get; init; }
			public decimal RandomVarScaleFactor { get; init; }
			public IContinuousDistribution RandomVariable { get; init; }
		}

		/// <summary>
		/// Utility Function to generate a Math.NET Continuous Distribution for use in Monte Carlo. 
		/// Parameters for the distribution should be added to the parameters dictionary with the key 
		/// for the respective variable. At the moment, the following keys are considered valid for 
		/// setting up distributions:
		/// <br/>
		/// - "Mu" - Mu parameter (used in Normal, LogNormal, and Student T distributions)<br/>
		/// - "Sigma" - Sigma Parameter (used in Normal, LogNormal, and Student T distributions)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		private static IContinuousDistribution CreateRandomVarInstance(MonteCarloRV type, Dictionary<string, double> parameters) {
			switch(type) {
				case MonteCarloRV.NORMAL:
					return new Normal(parameters["Mu"], parameters["Sigma"]);
				case MonteCarloRV.LOGNORMAL:
					return new LogNormal(parameters["Mu"], parameters["Sigma"]);
				default:
					throw new NotImplementedException();
			}
		}

		public static List<decimal> MonteCarloSim_SingleIteration(MonteCarloOptions options) {
			var currentPrice = options.BasePrice;
			var iterModel = new List<decimal>();

			for(var step = 0; step < options.AnalysisLength; step++) {
				iterModel.Add(currentPrice);
				currentPrice += (options.RandomVarScaleFactor
								* (decimal)(options.RandomVariable.Sample()));
			}

			return iterModel;
		}


		/// <summary>
		/// Monte Carlo Simulation using a "scaled" normal random variable to simulate the random walk. <br/>
		/// This is used to only simulate the random walk of the stock's price over time, post-processing is
		/// required for getting actual value of the stock
		/// <br/>
		/// Used Analysis Options: <br/>
		/// - "AnalysisLength": Number of Months to Simulate Analysis Over<br/>
		/// - "SimCount": Number of Simulations to perform <br/>
		/// - "RandomVariableMu": The Expectation (mu) of the Normal Distribution <br/>
		/// - "RandomVariableSigma": The Standard Deviation (sigma) of the Normal Distribution <br/>
		/// - "RandomVarialbeScaleFactor": The "scaling factor" to apply 
		/// to random variable samples. This is parsed as a <see cref="decimal"/>.
		/// </summary>
		/// <param name="stock"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static InvestmentModel MonteCarloSim_Normal(StockInvestment stock, OptionsDict options) {
			//Extract Required Data for simulation purposes
			var rvOptions = new Dictionary<string, double>() {
				["Mu"] = double.Parse(options["RandomVariableMu"]),
				["Sigma"] = double.Parse(options["RandomVariableSigma"])
			};
			var simOptions = new MonteCarloOptions() {
				BasePrice = stock.StockPrice,
				AnalysisLength = int.Parse(options["AnalysisLength"]),
				RandomVarScaleFactor = decimal.Parse(options["RandomVariableScaleFactor"]),
				RandomVariable = CreateRandomVarInstance(MonteCarloRV.NORMAL, rvOptions)
			};
			var maxIterations = int.Parse(options["SimCount"]);

			//Threading the task because .NET concurrency pretty sick
			var simLists = new ConcurrentBag<List<decimal>>();
			Parallel.For(0, maxIterations, x => {
				simLists.Add(MonteCarloSim_SingleIteration(simOptions));
			});

			var model = new InvestmentModel();
			for(int i = 0; i < simOptions.AnalysisLength; i++) {
				model.MinModelData.Add(simLists.Select(x => x[i]).Min());
				model.MaxModelData.Add(simLists.Select(x => x[i]).Max());
				model.AvgModelData.Add(simLists.Select(x => x[i]).Average());
			}

			return model;
		}

		/// <summary>
		/// Monte Carlo Simulation using a "scaled" lognormal random variable to simulate the random walk. <br/>
		/// This is used to only simulate the random walk of the stock's price over time, post-processing is
		/// required for getting actual value of the stock
		/// <br/>
		/// Used Analysis Options: <br/>
		/// - "AnalysisLength": Number of Months to Simulate Analysis Over<br/>
		/// - "SimCount": Number of Simulations to perform <br/>
		/// - "RandomVariableMu": The Expectation (mu) of the Normal Distribution <br/>
		/// - "RandomVariableSigma": The Standard Deviation (sigma) of the Normal Distribution <br/>
		/// - "RandomVarialbeScaleFactor": The "scaling factor" to apply 
		/// to random variable samples. This is parsed as a <see cref="decimal"/>.
		/// </summary>
		/// <param name="stock"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static InvestmentModel MonteCarloSim_LogNormal(StockInvestment stock, OptionsDict options) {
			//Extract Required Data for simulation purposes
			var rvOptions = new Dictionary<string, double>() {
				["Mu"] = double.Parse(options["RandomVariableMu"]),
				["Sigma"] = double.Parse(options["RandomVariableSigma"])
			};
			var simOptions = new MonteCarloOptions() {
				BasePrice = stock.StockPrice,
				AnalysisLength = int.Parse(options["AnalysisLength"]),
				RandomVarScaleFactor = decimal.Parse(options["RandomVariableScaleFactor"]),
				RandomVariable = CreateRandomVarInstance(MonteCarloRV.LOGNORMAL, rvOptions)
			};
			var maxIterations = int.Parse(options["SimCount"]);

			//Threading the task because .NET concurrency pretty sick
			var simLists = new ConcurrentBag<List<decimal>>();
			Parallel.For(0, maxIterations, x => {
				simLists.Add(MonteCarloSim_SingleIteration(simOptions));
			});

			var model = new InvestmentModel();
			for(int i = 0; i < simOptions.AnalysisLength; i++) {
				model.MinModelData.Add(simLists.Select(x => x[i]).Min());
				model.MaxModelData.Add(simLists.Select(x => x[i]).Max());
				model.AvgModelData.Add(simLists.Select(x => x[i]).Average());
			}

			return model;
		}

		//NOTE I think this is wrong, purely scratch work that *could* be useful
		//public static List<(string, decimal)> MonteCarloSim_GeometricBrownianMotion(int numSteps, decimal startVal, Dictionary<string, string> simOptions) {
		//	var model = new List<(string, decimal)>();
		//	var step = 0;

		//	var stockMu = double.Parse(simOptions["stockMu"]);
		//	var stockSigma = double.Parse(simOptions["stockSigma"]);
		//	var randomVarMu = double.Parse(simOptions["randomVarMu"]);
		//	var randomVarSigma = double.Parse(simOptions["randomVarSigma"]);
		//	var stockPrice = double.Parse(simOptions["stockPrice"]);

		//	//NOTE Can't use lognormal as that distribution can only be positive
		//	var randomVar = new Normal(randomVarMu, randomVarSigma);


		//	model.Add((step.ToString(), startVal));

		//	while(step < numSteps) {
		//		var drift = stockMu * step;
		//		var shock = stockSigma * randomVar.Sample() * Math.Sqrt(step);
		//		var baseDelta = drift + shock;
		//		//var scaledDelta = stockPrice * baseDelta; 

		//		//var modelVal = stockPrice + scaledDelta;

		//		model.Add((step.ToString(), (decimal)(stockPrice + baseDelta)));

		//		step++;
		//	}


		//	return model;
		//}

		//public static List<(string, decimal)> MonteCarloSim_GeometricBrownianMotion(int numSteps, double startVal, double mu, double sigma) {
		//	var model = new List<(string, decimal)>();
		//	var step = 0;
		//	var BrownianMotion = new Normal(mu, sigma);


		//	while(step < numSteps) {
		//		var exp_fact = (mu - (sigma * sigma) / 2) * step;
		//		exp_fact += sigma * BrownianMotion.Sample();
		//		var modelVal = startVal * Math.Exp(exp_fact);
		//		model.Add((step.ToString(), (decimal)modelVal));
		//		step++;
		//	}

		//	return model;
		//}


	}
}
