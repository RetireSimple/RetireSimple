using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

using RetireSimple.Engine.Analysis;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	public class Vehicle401k : InvestmentVehicleBase {
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


		//  ["pay freq"] = "weekly",
		//	["salary"] = "60000",
		//	["max_employer_salary_contribution"] = "0.06",
		//	["user_contribution_percent"] = "0.1",
		//	["employer_contribution_percent"] = "1.2",
		//	["user_contribution_type"] = "fixed",
		//	["user_monthly_contribution"] = "50",

		//	User_contribution = ["user_contribution_amount"] = 50
		//  Max_Employer_Contribution = 3600
		//	Employer_match = 50 * 1.20 = 60 
		//	Contribution_per_month = 50 + 60 = 160;

		public override List<decimal> SimulateCashContributions(OptionsDict options) {
			var analysisLength = int.Parse(options["analysisLength"]);
			var currentHoldings = this.CashHoldings;
			decimal contribution_multiplier = 0M;
	
			int pay_freq = 12;
			switch (options["pay freq"]) {
				case "monthly":
					pay_freq = 12;
					contribution_multiplier = 1;
					break;
				case "weekly":
					pay_freq = 52;
					contribution_multiplier = (13 / 3);
					break;    
				case "biweekly": 
					pay_freq = 26;
					contribution_multiplier = (13 / 6);
					break;
			}
			decimal salary = decimal.Parse(options["salary"]);
			decimal max_employer_salary_contribution = decimal.Parse(options["max_employer_salary_contribution"]);
			decimal user_contribution_percent = decimal.Parse(options["user_contribution_percent"]);
			decimal employer_contribution_percent = decimal.Parse(options["employer_contribution_percent"]);
			decimal user_contribution = 0M;

			if (options["user_contribution_type"] == "fixed") {
				user_contribution = decimal.Parse(options["user_contribution_amount"]);

			}
			else {
				user_contribution = salary / pay_freq * user_contribution_percent;
			}
			var max_employer_contribution = salary * max_employer_salary_contribution;
			var employer_match = user_contribution * employer_contribution_percent;
			if(employer_match >= (max_employer_contribution/pay_freq)) {
				employer_match = (max_employer_contribution/pay_freq);
			}
			var contribution_per_month = user_contribution + employer_match;
		
			return Enumerable.Range(0, analysisLength)
				.Select(idx => contribution_per_month * contribution_multiplier * idx)
				.Select(val => currentHoldings + val)
				.ToList();
		}

	}
}
