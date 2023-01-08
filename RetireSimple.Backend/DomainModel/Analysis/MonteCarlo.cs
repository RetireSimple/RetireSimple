using MathNet.Numerics.Distributions;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using System.Collections.Concurrent;

namespace RetireSimple.Backend.DomainModel.Analysis {
	public class MonteCarlo {

		//NOTE A lot of this is commented out intentionally as there is uncertainty on the
		//     best way to implement this. I'm leaving it here for now as a reference

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
		public static InvestmentModel MonteCarloSim_NormalDistribution(StockInvestment stock, OptionsDict options) {
			//Extract Required Data for simulation purposes
			var basePrice = stock.StockPrice;
			var analysisLength = int.Parse(options["AnalysisLength"]);
			var maxIterations = int.Parse(options["SimCount"]);
			var normalMu = double.Parse(options["RandomVariableMu"]);
			var normalSigma = double.Parse(options["RandomVariableSigma"]);
			var normalScaleFactor = decimal.Parse(options["RandomVariableScaleFactor"]);

			var normalDist = new Normal(normalMu, normalSigma);

			//Threading the task because .NET concurrency pretty sick
			var simLists = new ConcurrentBag<List<decimal>>();
			Parallel.For(0, maxIterations, x => {
				var currentPrice = basePrice;
				var iterModel = new List<decimal>();

				for(var step = 0; step < analysisLength; step++) {
					iterModel.Add(currentPrice);
					currentPrice += (normalScaleFactor * (decimal)normalDist.Sample());
				}

				simLists.Add(iterModel);
			});

			var model = new InvestmentModel();
			for(int i = 0; i < analysisLength; i++) {
				model.AvgModelData.Add(simLists.Select(x => x[i]).Min());
				model.AvgModelData.Add(simLists.Select(x => x[i]).Max());
				model.AvgModelData.Add(simLists.Select(x => x[i]).Average());
			}


			return model;
		}



		//public static List<(string, decimal)> MonteCarloSim_LogNormal(int numSteps, decimal startVal, Dictionary<string, string> simOptions) {
		//	var model = new List<(string, decimal)>();
		//	var step = 0;
		//	var mu = double.Parse(simOptions["mu"]);
		//	var sigma = double.Parse(simOptions["sigma"]);

		//	var distribution = new LogNormal(mu, sigma);

		//	model.Add((step.ToString(), startVal));


		//	return model;
		//}

		//NOTE I think this is wrong, purely scratch work
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


		//TODO encode this into a reasonable string and not steps!

		////Encodes periods to MMYYYY
		//public static string MakePeriodString(int month, int year) {
		//	return month.ToString("00") + year.ToString();
		//}

		//public static string ConvertStepToPeriod(int step) {
		//	var baseMonth = DateTime.Today.


		//}



	}
}
