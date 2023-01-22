using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class BondInvestment : InvestmentBase {

		public AnalysisDelegate<BondInvestment>? analysis;

		[JsonIgnore, NotMapped]
		public string BondName {
			get => this.InvestmentData["BondName"];
			set => this.InvestmentData["BondName"] = value;
		}

		[JsonIgnore, NotMapped]
		// interest payment received by a bondholder from the date of issuance until the date of maturity
		public decimal BondCouponRate {
			get => decimal.Parse(this.InvestmentData["BondCouponRate"]);
			set => this.InvestmentData["BondCouponRate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly BondMaturityDate {
			get => DateOnly.Parse(this.InvestmentData["BondMaturityDate"]);
			set => this.InvestmentData["BondMaturityDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal BondPurchasePrice {
			get => decimal.Parse(this.InvestmentData["BondPurchasePrice"]);
			set => this.InvestmentData["BondPurchasePrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public DateOnly BondPurchaseDate {
			get => DateOnly.Parse(this.InvestmentData["BondPurchaseDate"]);
			set => this.InvestmentData["BondPurchaseDate"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal BondCurrentPrice {
			get => decimal.Parse(this.InvestmentData["BondCurrentPrice"]);
			set => this.InvestmentData["BondCurrentPrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisDelegate<BondInvestment>? AnalysisMethod;

		//Constructor used by EF
		public BondInvestment(String analysisType) : base() {
			InvestmentType = "BondInvestment";
			ResolveAnalysisDelegate(analysisType);

		}

		public override void ResolveAnalysisDelegate(string analysisType) {
			switch(analysisType) {
				case "testAnalysis":
					AnalysisMethod = BondAS.DefaultBondAnalysis;
					break;
				default:
					AnalysisMethod = null;
					break;
			}

			//Overwrite The current Analysis Delegate Type 
			this.AnalysisType = analysisType;
		}
		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			(AnalysisMethod is not null)
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}