using RetireSimple.Engine.Analysis;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {
	public class BondInvestment : InvestmentBase {

		[JsonIgnore, NotMapped]
		public string BondTicker {
			get => InvestmentData["bondTicker"];
			set => InvestmentData["bondTicker"] = value;
		}

		[JsonIgnore, NotMapped]
		// interest payment received by a bondholder from the date of issuance until the date of maturity
		public double BondCouponRate {
			get => double.Parse(InvestmentData["bondCouponRate"]);
			set => InvestmentData["bondCouponRate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		// interest payment received by a bondholder from the date of issuance until the date of maturity
		public decimal BondYTM {
			get => decimal.Parse(InvestmentData["bondYieldToMaturity"]);
			set => InvestmentData["bondYieldToMaturity"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly BondMaturityDate {
			get => DateOnly.Parse(InvestmentData["bondMaturityDate"]);
			set => InvestmentData["bondMaturityDate"] = value.ToString("yyyy-MM-dd");
		}

		[JsonIgnore, NotMapped]
		public decimal BondPurchasePrice {
			get => decimal.Parse(InvestmentData["bondPurchasePrice"]);
			set => InvestmentData["bondPurchasePrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly BondPurchaseDate {
			get => DateOnly.Parse(InvestmentData["bondPurchaseDate"]);
			set => InvestmentData["bondPurchaseDate"] = value.ToString("yyyy-MM-dd");
		}

		[JsonIgnore, NotMapped]
		public decimal BondCurrentPrice {
			get => decimal.Parse(InvestmentData["bondCurrentPrice"]);
			set => InvestmentData["bondCurrentPrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public bool BondIsAnnual {
			get => ToBoolean(InvestmentData["bondIsAnnual"]);
			set => InvestmentData["bondIsAnnual"] = value.ToString().ToLower();
		}

		private static bool ToBoolean(string str) => str.Equals("true");


		[JsonIgnore, NotMapped]
		public AnalysisDelegate<BondInvestment>? AnalysisMethod;

		//Constructor used by EF
		public BondInvestment(string analysisType) : base() {
			InvestmentType = "BondInvestment";
			ResolveAnalysisDelegate(analysisType);

		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			AnalysisMethod = analysisType switch {
				"testAnalysis" => BondAS.DefaultBondAnalysis,
				_ => null,
			};

			//Overwrite The current Analysis Delegate Type
			AnalysisType = analysisType;
		}
		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}