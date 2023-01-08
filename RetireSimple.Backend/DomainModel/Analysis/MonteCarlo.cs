using MathNet.Numerics.Distributions;
using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

namespace RetireSimple.Backend.DomainModel.Analysis {
	public class MonteCarlo {

		//NOTE A lot of this is commented out intentionally as there is uncertainty on the
		//     best way to implement this. I'm leaving it here for now as a reference

		public static InvestmentModel MonteCarloSim_NormalDistribution(StockInvestment stock, OptionsDict options) {
			//TODO implement



			
			return null;
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
