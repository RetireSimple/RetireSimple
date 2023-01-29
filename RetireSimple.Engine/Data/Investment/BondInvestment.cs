using RetireSimple.Engine.Analysis;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {
	public class BondInvestment : InvestmentBase {

		public AnalysisDelegate<BondInvestment>? analysis;

		[JsonIgnore, NotMapped]
		// interest payment received by a bondholder from the date of issuance until the date of maturity
		public decimal BondCouponRate {
			get => decimal.Parse(InvestmentData["BondCouponRate"]);
			set => InvestmentData["BondCouponRate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly BondMaturityDate {
			get => DateOnly.Parse(InvestmentData["BondMaturityDate"]);
			set => InvestmentData["BondMaturityDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal BondPurchasePrice {
			get => decimal.Parse(InvestmentData["BondPurchasePrice"]);
			set => InvestmentData["BondPurchasePrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly BondPurchaseDate {
			get => DateOnly.Parse(InvestmentData["BondPurchaseDate"]);
			set => InvestmentData["BondPurchaseDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal BondCurrentPrice {
			get => decimal.Parse(InvestmentData["BondCurrentPrice"]);
			set => InvestmentData["BondCurrentPrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisDelegate<BondInvestment>? AnalysisMethod;

		//Constructor used by EF
		public BondInvestment(string analysisType) : base() {
			InvestmentType = "BondInvestment";
			ResolveAnalysisDelegate(analysisType);

		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch (analysisType) {
				case "testAnalysis":
					AnalysisMethod = BondAS.DefaultBondAnalysis;
					break;
				default:
					AnalysisMethod = null;
					break;
			}

			//Overwrite The current Analysis Delegate Type
			AnalysisType = analysisType;
		}
		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}