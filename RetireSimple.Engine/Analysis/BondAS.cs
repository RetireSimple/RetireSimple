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

		[AnalysisModule("BondInvestment")]
		public static InvestmentModel StdBondValuation(BondInvestment investment, OptionsDict options) {
			var list = BondValuation(investment, options);
			return new InvestmentModel() {
				MinModelData = list,
				AvgModelData = list,
				MaxModelData = list,
				InvestmentId = investment.InvestmentId,
				LastUpdated = DateTime.Now,
			};
		}

		public static readonly OptionsDict DefaultBondAnalysisOptions = new() {
			["analysisLength"] = "60",                    //Number of months to project
			["isAnnual"] = "true",
		};

		public static List<decimal> BondValuation(BondInvestment investment, OptionsDict options) {
			var bondVals = new List<decimal>();

			DateOnly purchaseDate = investment.BondPurchaseDate;
			DateOnly maturityDate = investment.BondMaturityDate;
			DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
			int analysisLength = int.Parse(investment.AnalysisOptionsOverrides["analysisLength"]);
			int monthsApart = Math.Abs(12 * ((purchaseDate.Year - maturityDate.Year) + (purchaseDate.Month - maturityDate.Month)));
			int monthInterval = 12;
			decimal faceVal = investment.BondFaceValue;
			int n = 1;
			int k = 1;
			decimal discountRate = investment.BondYTM;
			decimal cashFlow = faceVal * (decimal)investment.BondCouponRate;
			decimal presentVal = 0;

			if (maturityDate < currentDate) {
				bondVals = Enumerable.Repeat((cashFlow * monthsApart) + faceVal, analysisLength).ToList();
				return bondVals;
			}

			if (!bool.Parse(investment.AnalysisOptionsOverrides["isAnnual"])) {
				monthInterval = 6;
			}

			while (n <= monthsApart) {
				if (n % monthInterval == 0) {
					if (n - monthsApart == 0) {
						cashFlow += faceVal;
					}
					presentVal += (decimal)((double)cashFlow / Math.Pow(1 + (double)discountRate, k));
					k++;
				}
				bondVals.Add(presentVal);
				n++;
			}
			var tempList = new List<decimal>();

			//Date Comparisons
			if (purchaseDate <= currentDate) {
				int dateDiff = Math.Abs(12 * (currentDate.Year - purchaseDate.Year) + (currentDate.Month - purchaseDate.Month));
				tempList = bondVals.Take(new Range(dateDiff, analysisLength + dateDiff)).ToList();
			}
			else if (purchaseDate > currentDate) {
				int dateDiff = Math.Abs(12 * (purchaseDate.Year - currentDate.Year) + purchaseDate.Month - currentDate.Month);
				var zeroList = new List<decimal>();
				zeroList.AddRange(Enumerable.Repeat(0M, dateDiff));
				zeroList.AddRange(bondVals);
				tempList = zeroList.Take(new Range(0, analysisLength)).ToList();
			}
			if (tempList.Count < analysisLength) {
				tempList.AddRange(Enumerable.Repeat(tempList.Last(), analysisLength - tempList.Count));
			}

			return tempList;
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