using MathNet.Numerics.Distributions;

using RetireSimple.Engine.Data.Investment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.Engine.Analysis {
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
		PCR,	//Principal Components Regression
		PLSR,	//Partial Least Squares Regression
		ENR,	//Elastic Net Regression

	}

	private static IContinuousDistribution CreateRandomVarInstance(MonteCarloRV type, Dictionary<string, double> parameters) {
		return type switch {
			RegressionRV.LINEAR => new 
			_ => throw new NotImplementedException(),
		};
	}
	internal class Regression {

		public static List<decimal> LinearRegression(BondInvestment investment, OptionsDict options) {
			// Get the list of bond prices and yields from the investment object
			List<double> bondPrices = investment.BondPrices;
			List<double> yields = investment.Yields;

			// Check if the lists have the same length
			if (bondPrices.Count != yields.Count) {
				throw new ArgumentException("The bond prices and yields lists must have the same length.");
			}

			// Convert the lists to arrays
			double[] pricesArray = bondPrices.ToArray();
			double[] yieldsArray = yields.ToArray();

			// Calculate the slope and intercept of the linear regression
			double slope, intercept;
			LinearRegression(yieldsArray, pricesArray, out slope, out intercept);

			// Calculate the predicted bond prices for the given yields
			List<decimal> predictedPrices = new List<decimal>();
			foreach (decimal yield in options.Yields) {
				decimal predictedPrice = (decimal)(slope * (double)yield + intercept);
				predictedPrices.Add(predictedPrice);
			}

			// Return the list of predicted bond prices
			return predictedPrices;
		}

		// Helper method to perform linear regression
		public static void LinearRegression(double[] xVals, double[] yVals, out double slope, out double yIntercept) {
			int n = xVals.Length;
			double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

			for (int i = 0; i < n; i++) {
				sumX += xVals[i];
				sumY += yVals[i];
				sumXY += xVals[i] * yVals[i];
				sumX2 += xVals[i] * xVals[i];
			}

			double xMean = sumX / n;
			double yMean = sumY / n;

			double denominator = sumX2 - (sumX * sumX / n);

			if (denominator == 0) {
				slope = 0;
				yIntercept = yMean;
				return;
			}

			slope = (sumXY - sumX * yMean) / denominator;
			yIntercept = yMean - slope * xMean;
		}
	}
}
