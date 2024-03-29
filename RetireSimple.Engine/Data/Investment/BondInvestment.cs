using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment {

	[InvestmentModule(nameof(AnalysisMethod))]
	public class BondInvestment : Base.Investment {

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
		public DateOnly BondPurchaseDate {
			get => DateOnly.Parse(InvestmentData["bondPurchaseDate"]);
			set => InvestmentData["bondPurchaseDate"] = value.ToString("yyyy-MM-dd");
		}

		[JsonIgnore, NotMapped]
		public decimal BondFaceValue {
			get => decimal.Parse(InvestmentData["bondFaceValue"]);
			set => InvestmentData["bondFaceValue"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public decimal BondCurrentPrice {
			get => decimal.Parse(InvestmentData["bondCurrentPrice"]);
			set => InvestmentData["bondCurrentPrice"] = value.ToString();
		}

		[JsonIgnore, NotMapped]
		public AnalysisModule<BondInvestment>? AnalysisMethod { get; private set; }

		//Constructor used by EF
		public BondInvestment(string analysisType) : base(analysisType) { }

		//Constructor used by InvestmentApi
		public BondInvestment(OptionsDict parameters)
			: base(parameters.GetValueOrDefault("analysisType", "StdBondValuation")) {

			BondTicker = parameters.GetValueOrDefault("bondTicker", "N/A");
			BondCouponRate = double.Parse(parameters.GetValueOrDefault("bondCouponRate", "0.05"));
			BondYTM = decimal.Parse(parameters.GetValueOrDefault("bondYieldToMaturity", "0.05"));
			BondMaturityDate = DateOnly.FromDateTime(DateTime.Parse(parameters.GetValueOrDefault("bondMaturityDate",
																	DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"))));
			BondFaceValue = decimal.Parse(parameters.GetValueOrDefault("bondFaceValue", "0"));
			BondPurchaseDate = DateOnly.FromDateTime(DateTime.Parse(parameters.GetValueOrDefault("bondPurchaseDate",
																	 DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"))));
			BondCurrentPrice = decimal.Parse(parameters.GetValueOrDefault("bondCurrentPrice", "0"));
		}

		public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
			AnalysisMethod is not null
			? AnalysisMethod(this, options)
			: throw new InvalidOperationException("The specified investment has no specified analysis");
	}
}