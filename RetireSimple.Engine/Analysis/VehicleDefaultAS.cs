using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.InvestmentVehicle;

namespace RetireSimple.Engine.Analysis {

	public static class VehicleDefaultAS {

		public static InvestmentModel GeneratePostTaxModelDefaultAfterTaxVehicle(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {
			//We are basically using the pretax model and applying tax to understand value after tax.
			var preTaxModel =
				GeneratePreTaxModelDefaultAfterTaxVehicle(options, models, cashContribution);

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

		///The following methods provide common logic for implementing the template method. You can
		///have specific modules wrap around these if the logic applies (even partially).
		public static InvestmentModel GeneratePreTaxModelDefaultAfterTaxVehicle(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {
			//The logic here is a bit confusing at first, but here is an explanation of the transforms
			var minModel = Enumerable.Range(0, models[0].MinModelData.Count) //Project the total length of the model with indexes
									.Select(model =>                                    //For each index,
										models.Select(m => m.MinModelData[model]).Sum()); //project each model's value at that index and sum
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

		public static InvestmentModel GeneratePreTaxModelDefaultPreTaxVehicle(OptionsDict options,
			List<InvestmentModel> models,
			List<decimal>? cashContribution = null) {

			var taxPercentage = decimal.Parse(options["vehicleTaxPercentage"]);
			//The logic here is a bit confusing at first, but here is an explanation of the transforms
			var minModel = Enumerable.Range(0, models[0].MinModelData.Count)                //Project the total length of the model with indexes
									.Select(model =>                                        //For each index,
										models.Select(m => m.MinModelData[model]).Sum())    //project each model's value at that index and sum
									.Select(v => v * (1 - taxPercentage));                  // Apply Taxes
			var maxModel = Enumerable.Range(0, models[0].MaxModelData.Count)
									.Select(model =>
										models.Select(m => m.MaxModelData[model]).Sum())
									.Select(v => v * (1 - taxPercentage));
			var avgModel = Enumerable.Range(0, models[0].AvgModelData.Count)
									.Select(model =>
										models.Select(m => m.AvgModelData[model]).Sum())
									.Select(v => v * (1 - taxPercentage));

			//If cash-based contributions exist, transform all models to include them
			//This version assumes that cash contributions are already taxed
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

		public static List<decimal> SimulateCashContributionsDefaultAfterTax(InvestmentVehicleBase vehicle, OptionsDict options) {
			var analysisLength = int.Parse(options["analysisLength"]);
			var cashContribution = decimal.Parse(options["cashContribution"]);
			var currentHoldings = vehicle.CashHoldings;

			return Enumerable.Range(0, analysisLength)
				.Select((val, idx) => currentHoldings + cashContribution * idx)
				.ToList();
		}

		public static List<decimal> SimulateCashContributionsDefaultPreTax(InvestmentVehicleBase vehicle, OptionsDict options) {
			var analysisLength = int.Parse(options["analysisLength"]);
			var cashContribution = decimal.Parse(options["cashContribution"]);
			var taxPercentage = decimal.Parse(options["vehicleTaxPercentage"]);
			var currentHoldings = vehicle.CashHoldings;

			return Enumerable.Range(0, analysisLength)
				.Select((val, idx) => cashContribution * idx)
				.Select(val => val * (1 - taxPercentage))
				.Select(val => val + currentHoldings)
				.ToList();
		}
	}
}