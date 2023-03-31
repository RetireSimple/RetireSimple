using RetireSimple.Engine.Analysis;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace RetireSimple.Engine.Data.Investment {
	public class CashInvestment : Base.Investment {

		[JsonIgnore, NotMapped]
		public string CashCurrency {
			get => InvestmentData["CashCurrency"];
			set => InvestmentData["CashCurrency"] = value;
		}

		[JsonIgnore, NotMapped]
		public decimal CashQuantity {
			get => decimal.Parse(InvestmentData["CashQuantity"]);
			set => InvestmentData["CashQuantity"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal CashCurrentValue {
			get => decimal.Parse(InvestmentData["CashValue"]);
			set => InvestmentData["CashValue"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisModule<CashInvestment>? AnalysisMethod { get; private set; }

		public CashInvestment(string analysisType) : base() {
			InvestmentType = "CashInvestment";
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}
