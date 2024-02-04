using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Engine.Analysis.Utils {
	public enum RegressionRV {
		BINOMIAL,
		//The following are currently not implemented, but are supported by Math.NET
		LINEAR,
		LOGISTIC,
		POLYNOMIAL,
		RIDGE,
		LASSO,
		QUANTILE,
		BAYESIAN,
		PCR,    //Principal Components Regression
		PLSR,   //Partial Least Squares Regression
		ENR,    //Elastic Net Regression
	}

	/// <summary>
	///Binomial Regression Utility for Stock Analysis
	/// </summary>
	public class Regression {

		internal decimal BasePrice { get; init; }
		internal int AnalysisLength { get; init; }
		internal decimal Uncertainty { get; init; }
		internal decimal ExpectedGrowth { get; init; }

		public Regression(OptionsDict options) {
			BasePrice = Math.Max(decimal.Parse(options["basePrice"]), 0);
			AnalysisLength = Math.Max(int.Parse(options["analysisLength"]), 0);
			Uncertainty = Math.Max(decimal.Parse(options["uncertainty"]), 0);
			ExpectedGrowth = decimal.Parse(options["percentGrowth"]);
		}

		public InvestmentModel RunSimulation() {

			var model = new InvestmentModel();
			var initialPrice = BasePrice;
			//Will the potential growth always mirror the potenial risk
			var u = 1 + ExpectedGrowth;
			var d = 1 - ExpectedGrowth;

			// Initialize strike price tree
			List<List<decimal>> striketree = new List<List<decimal>>();
			List<decimal> strikecolumn = new List<decimal> {
				initialPrice
			};
			striketree.Add(strikecolumn);

			// produce strike price tree see example above
			for (int i = 0; i < AnalysisLength; i++) {
				strikecolumn = new List<decimal>();
				foreach (decimal strike in striketree[i]) {
					strikecolumn.Add(strike * u);
					strikecolumn.Add(strike * d);
				}
				var strikeMax = strikecolumn.Max();
				var strikeMin = strikecolumn.Min();

				strikecolumn.Clear();
				strikecolumn.Add(strikeMax);
				strikecolumn.Add(strikeMin);

				striketree.Add(strikecolumn);
			}

			foreach (List<decimal> val in striketree) {
				model.MinModelData.Add(val.Min());
				model.AvgModelData.Add(val.Average());
				model.MaxModelData.Add(val.Max());
			}

			return model;
		}
	}
}
