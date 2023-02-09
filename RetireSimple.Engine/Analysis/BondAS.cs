using MathNet.Numerics;

using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

using System;
using System.Diagnostics.Metrics;
using System.Drawing;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RetireSimple.Engine.Analysis {

	public class BondAS {

		public static InvestmentModel DefaultBondAnalysis(BondInvestment investment, OptionsDict options) {
			throw new NotImplementedException();
		}

		public static readonly OptionsDict DefaultBondAnalysisOptions = new() {
			["AnalysisLength"] = "60",                          //Number of months to project
			["isAnnual"] = "true",
		};
		// Coupon value = sum (future cash flow / (1 + yeild to maturity)^number of periods)
		// Ex. bond: face value of $1,000 and annual coupon of three percent and a maturity date in 30 years.
		// company or country that owes the bond will pay the bondholder three percent of the face value of $1,000 ($30) every year
		// for 30 years, at which point they will pay the bondholder the full $1,000 face value.
		// You would have a series of 30 cash flows (one each year of $30) and then one cash flow (30 years from now, of $1,000)
		public static double PeriodicCashFlow(BondInvestment investment, OptionsDict options) {
			double couponRate = investment.BondCouponRate;

			if (investment.BondIsAnnual) {
				return (double)investment.BondPurchasePrice * couponRate;
			}
			else { //Bond is semi-annual
				return (double)investment.BondPurchasePrice * (couponRate / 2);
			}
		}

		// Face value = bond face value / (1 + yeild to maturity)^Time to maturity
		public static double CurrentBondValue(BondInvestment investment, OptionsDict options) {
			DateTime MaturityDateTime = investment.BondMaturityDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
			DateTime StartDateTime = investment.BondPurchaseDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
			int yearsToMaturity = MaturityDateTime.Subtract(StartDateTime).Days % 365;
			int numMaturityPeriods = yearsToMaturity;
			double faceVal = (double)investment.BondPurchasePrice;
			double discountRate = yearsToMaturity;

			if (!investment.BondIsAnnual) { //Bond is semi-annual
				numMaturityPeriods = yearsToMaturity * 2;
				discountRate = (double)yearsToMaturity / 2;
			}

			return faceVal / Math.Pow(1 + discountRate, numMaturityPeriods);
		}


		public static OptionsDict MergeAnalysisOption(BondInvestment investment, OptionsDict dict) {
			var newDict = new OptionsDict(dict);
			var investmentOptions = investment.AnalysisOptionsOverrides;

			foreach (var k in investmentOptions.Keys) {
				newDict.TryAdd(k, investmentOptions[k]);
			}

			foreach (var k in DefaultBondAnalysisOptions.Keys) {
				newDict.TryAdd(k, DefaultBondAnalysisOptions[k]);
			}

			return newDict;
		}

	}
}