namespace RetireSimple.Engine.Data.InvestmentVehicle {
	public class VehicleIRA : InvestmentVehicleBase {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> GeneratePreTaxModelDefaultAfterTaxVehicle(options, models, cashContribution);
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> GeneratePostTaxModelDefaultAfterTaxVehicle(options, models, cashContribution);
		public override List<decimal> SimulateCashContributions(OptionsDict options)
			=> SimulateCashContributionsDefaultAfterTax(options);
	}
}
