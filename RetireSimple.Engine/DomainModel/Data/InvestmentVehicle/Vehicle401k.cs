namespace RetireSimple.Engine.DomainModel.Data.InvestmentVehicle {
	public class Vehicle401k : InvestmentVehicleBase {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> GeneratePreTaxModel_DefaultAfterTaxVehicle(options, models, cashContribution);
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> GeneratePostTaxModel_DefaultAfterTaxVehicle(options, models, cashContribution);
		public override List<decimal> SimulateCashContributions(OptionsDict options)
			=> SimulateCashContributions_DefaultAfterTax(options);
	}
}
