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
			["AnalysisLength"] = "240",                          //Number of months to project
			["isAnnual"] = "true",
		};

		public static List<decimal> BondValuation(BondInvestment investment, OptionsDict options) {
			var bondVals = new List<decimal>();
			int monthsApart = Math.Abs(12 * (investment.BondPurchaseDate.Year - investment.BondMaturityDate.Year) + investment.BondPurchaseDate.Month - investment.BondMaturityDate.Month);
			int monthInterval = 12;
			decimal faceVal = investment.BondFaceValue;
			int n = 1;
			int k = 1;
			decimal discountRate = investment.BondYTM;
			decimal cashFlow = faceVal * (decimal)investment.BondCouponRate;
			decimal presentVal = 0;
		
			if (!(investment.BondIsAnnual.Equals("Annual"))) {
				monthInterval = 6;
			}

			while(n < monthsApart+1) { 
				if (n % monthInterval == 0) { 
					if(n-monthsApart==0) { 
						cashFlow += faceVal;
					}
					presentVal += (decimal)((double)cashFlow/ Math.Pow(1 + (double)discountRate, k));
					bondVals.Add(presentVal);
					k++;
				}
				n++;
			}

			return bondVals;
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