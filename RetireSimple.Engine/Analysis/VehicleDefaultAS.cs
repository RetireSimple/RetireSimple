using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Engine.Analysis {

	public static class VehicleDefaultAS {

		public static InvestmentModel GeneratePostTaxModelDefault(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {

			var shortTermTax = decimal.Parse(options["shortTermCapitalGainsTax"]);
			var longTermTax = decimal.Parse(options["longTermCapitalGainsTax"]);

			List<InvestmentModel> newModels = new List<InvestmentModel>();

			foreach (var model in models) {
				//var newModel = new InvestmentModel();
				var minModel = new List<decimal>();
				var avgModel = new List<decimal>();
				var maxModel = new List<decimal>();

				var capitalGainsMin = 0M;
				var capitalGainsMax = 0M;
				var capitalGainsAvg = 0M;

				for (int i = 0; i < int.Parse(options["analysisLength"]); i++) {
					decimal tax = shortTermTax;
					// first 12 values qualify for short term capital gains tax
					if (i < 12) {
						tax = shortTermTax;
					} else {  // everything after (>11) is subject to long term capital gains
						tax = longTermTax;
					}
					capitalGainsMin = Math.Max(0, model.MinModelData[i] - model.MinModelData[0]);
					capitalGainsMin *= 1 - tax;
					minModel.Add(capitalGainsMin + model.MinModelData[i]);

					capitalGainsMax = Math.Max(0, model.MaxModelData[i] - model.MaxModelData[0]);
					capitalGainsMax *= 1 - tax;
					maxModel.Add(capitalGainsMax + model.MaxModelData[i]);

					capitalGainsAvg = Math.Max(0, model.AvgModelData[i] - model.AvgModelData[0]);
					capitalGainsAvg *= 1 - tax;
					avgModel.Add(capitalGainsAvg + model.AvgModelData[i]);
				}
				newModels.Add(new InvestmentModel() {
					MinModelData = minModel,
					MaxModelData = maxModel,
					AvgModelData = avgModel,
				});
			}

			var aggregateModel = new InvestmentModel() {
				MinModelData = Enumerable.Range(0, int.Parse(options["analysisLength"]))
					.Select(modelidx => newModels.Select(m => m.MinModelData[modelidx]).Sum()).ToList(),
				MaxModelData = Enumerable.Range(0, int.Parse(options["analysisLength"]))
					.Select(modelidx => newModels.Select(m => m.MaxModelData[modelidx]).Sum()).ToList(),
				AvgModelData = Enumerable.Range(0, int.Parse(options["analysisLength"]))
					.Select(modelidx => newModels.Select(m => m.AvgModelData[modelidx]).Sum()).ToList(),
			};

			// re-add cash contributions
			if (cashContribution != null) {
				aggregateModel.MinModelData = aggregateModel.MinModelData.Select((val, idx) => val + cashContribution[idx]).ToList();
				aggregateModel.MaxModelData = aggregateModel.MaxModelData.Select((val, idx) => val + cashContribution[idx]).ToList();
				aggregateModel.AvgModelData = aggregateModel.AvgModelData.Select((val, idx) => val + cashContribution[idx]).ToList();
			}
			return aggregateModel;
		}

		///The following methods provide common logic for implementing the template method. You can
		///have specific modules wrap around these if the logic applies (even partially).
		public static InvestmentModel GeneratePreTaxModelDefault(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {
			//The logic here is a bit confusing at first, but here is an explanation of the transforms
			var minModel = Enumerable.Range(0, models[0].MinModelData.Count)                // Project the total length of the model with indexes
									.Select(model =>                                        // For each index,
										models.Select(m => m.MinModelData[model]).Sum());   // project each model's value at that index and sum
			var maxModel = Enumerable.Range(0, models[0].MaxModelData.Count)
									.Select(model =>
										models.Select(m => m.MaxModelData[model]).Sum());
			var avgModel = Enumerable.Range(0, models[0].AvgModelData.Count)
									.Select(model =>
										models.Select(m => m.AvgModelData[model]).Sum());

			//If cash-based contributions exist, transform all models to include them
			if (cashContribution != null) {
				minModel = minModel.Select((val, idx) => val + cashContribution[idx]);
				maxModel = maxModel.Select((val, idx) => val + cashContribution[idx]);
				avgModel = avgModel.Select((val, idx) => val + cashContribution[idx]);
			}

			var preTaxModel = new InvestmentModel() {
				MinModelData = minModel.ToList(),
				MaxModelData = maxModel.ToList(),
				AvgModelData = avgModel.ToList()
			};

			return preTaxModel;
		}

		public static List<decimal> SimulateCashContributionsDefault(InvestmentVehicle vehicle, OptionsDict options) {
			var analysisLength = int.Parse(options["analysisLength"]);
			var cashContribution = decimal.Parse(options["userContributionAmount"]);
			var currentHoldings = vehicle.CashHoldings;

			return Enumerable.Range(0, analysisLength)
				.Select((val, idx) => currentHoldings + cashContribution * idx)
				.ToList();
		}

		public static List<decimal> SimulateCashContributions401kLike(InvestmentVehicle vehicle, OptionsDict options) {
			var analysisLength = int.Parse(options["analysisLength"]);
			var currentHoldings = vehicle.CashHoldings;
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