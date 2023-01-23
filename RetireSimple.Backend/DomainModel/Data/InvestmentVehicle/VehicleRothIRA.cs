namespace RetireSimple.Backend.DomainModel.Data.InvestmentVehicle {
	public class VehicleRothIRA : InvestmentVehicleBase {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options,
													List<InvestmentModel> models,
													List<decimal>? cashContribution = null)
			=> GeneratePreTaxModel_DefaultPreTaxVehicle(options, models, cashContribution);
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> GeneratePreTaxModel_DefaultPreTaxVehicle(options, models, cashContribution);
		public override List<decimal> SimulateCashContributions(OptionsDict options)
			=> SimulateCashContributions_DefaultPreTax(options);
	}
}
