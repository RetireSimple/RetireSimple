using RetireSimple.Engine.Data.Analysis;

using System;
using System.Collections.Concurrent;


namespace RetireSimple.Engine.Analysis.Utils {
	public enum RegressionRV {
		LINEAR,
		//The following are currently not implemented, but are supported by Math.NET
		LOGISTIC,
		POLYNOMIAL,
		RIDGE,
		LASSO,
		QUANTILE,
		BAYESIAN,
		Bayesian,
		PCR,    //Principal Components Regression
		PLSR,   //Partial Least Squares Regression
		ENR,    //Elastic Net Regression
	}

	public class Regression {

		internal decimal BasePrice { get; init; }
		internal int AnalysisLength { get; init; }
		internal decimal Uncertainty { get; init; }
		internal decimal ExpectedGrowth { get; init; }
		internal int Quantity { get; init; }


		/// <summary>
		/// Utility Function to generate a Math.NET Continuous Distribution for use in Regression analysis.
		/// <param name="type"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>

		//internal static icontinuousdistribution createrandomvarinstance(montecarlorv type, dictionary<string, double> parameters) {
		//	return type switch {
		//		regressionrv.logistic =>
		//		_ => throw new notimplementedexception(),
		//	};
		//}
		public Regression(OptionsDict options) {
			Quantity = int.Parse(options["stockQuantity"]);
			BasePrice = decimal.Parse(options["basePrice"]);
			AnalysisLength = int.Parse(options["analysisLength"]);
			Uncertainty = decimal.Parse(options["uncertainty"]);
			ExpectedGrowth = decimal.Parse(options["percentGrowth"]);
		}

		public InvestmentModel RunSimulation() {
			
			var model = new InvestmentModel();
			var price = BasePrice * Quantity;
			var finalPrice = price + (price * ExpectedGrowth);
			decimal[] residuals = new decimal[60];

			// Generate random residuals with normal distribution
			Random random = new Random(1);
			for (int i = 0; i < AnalysisLength; i++) {
				double u1 = 1.0 - random.NextDouble();
				double u2 = 1.0 - random.NextDouble();
				double normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
				residuals[i] = 0 + Uncertainty * (decimal)normal;
			}
			decimal mingrowth = 0;
			decimal avggrowth = 0;
			decimal maxgrowth = 0;

			// Compute predicted prices
			decimal[] predictedPrices = new decimal[60];
			for (int i = 0; i < 60; i++) {
				mingrowth = ExpectedGrowth - Uncertainty;
				avggrowth = ExpectedGrowth;
				maxgrowth = ExpectedGrowth + Uncertainty;
				model.MinModelData.Add(price * (1 + mingrowth * (i + 1)) * (1 + residuals[i]));
				model.MaxModelData.Add(price * (1 + maxgrowth * (i + 1)) * (1 + residuals[i]));
				model.AvgModelData.Add(price * (1 + avggrowth * (i + 1)) * (1 + residuals[i]));
			}

			return model;
			}
	}
}
