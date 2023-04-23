using RetireSimple.Engine.Analysis;
using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	[InvestmentVehicleModule]
	public class Vehicle401k : Base.InvestmentVehicle {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options, List<InvestmentModel> models, List<decimal>? cashContribution = null) {
			var preTaxModel = GeneratePreTaxModels(options, models, cashContribution);

			var taxPercentage = decimal.Parse(options["vehicleTaxPercentage"]);
			var postTaxModel = new InvestmentModel() {
				MinModelData = preTaxModel.MinModelData.Select(v => v * (1 - taxPercentage))
					.ToList(),
				MaxModelData = preTaxModel.MaxModelData.Select(v => v * (1 - taxPercentage))
					.ToList(),
				AvgModelData = preTaxModel.AvgModelData.Select(v => v * (1 - taxPercentage))
					.ToList()
			};
			return postTaxModel;
		}

		public override InvestmentModel GeneratePreTaxModels(OptionsDict options, List<InvestmentModel> models, List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePreTaxModelDefault(options, models, cashContribution);

		public override List<decimal> SimulateCashContributions(OptionsDict options) {
			var analysisLength = int.Parse(options["analysisLength"]);
			var currentHoldings = this.CashHoldings;
			decimal contribution_multiplier = 0M;

			int pay_freq = 12;
			switch (options["payFrequency"]) {
				case "monthly":
					pay_freq = 12;
					contribution_multiplier = 1;
					break;
				case "weekly":
					pay_freq = 52;
					contribution_multiplier = (52 / 12);
					break;
				case "biweekly":
					pay_freq = 26;
					contribution_multiplier = (52 / 24);
					break;
			}
			decimal salary = decimal.Parse(options["salary"]);
			decimal userContributionPercentage = decimal.Parse(options["userContributionPercentage"]);
			decimal employerMatchPercentage = decimal.Parse(options["employerMatchPercentage"]);
			decimal userContribution = 0M;

			if (options["userContributionType"] == "fixed") {
				userContribution = decimal.Parse(options["userContributionAmount"]);
			} else {
				userContribution = salary / pay_freq * userContributionPercentage;
			}
			var employer_match = userContribution * employerMatchPercentage;
			var contribution_per_month = userContribution + employer_match;

			return Enumerable.Range(0, analysisLength)
				.Select(idx => contribution_per_month * contribution_multiplier * idx)
				.Select(val => currentHoldings + val)
				.ToList();
		}

	}
}
