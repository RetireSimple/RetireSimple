using RetireSimple.Engine.Analysis;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	public class VehicleRothIRA : InvestmentVehicleBase {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options,
													List<InvestmentModel> models,
													List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePreTaxModelDefaultPreTaxVehicle(options, models, cashContribution);
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePreTaxModelDefaultPreTaxVehicle(options, models, cashContribution);
		public override List<decimal> SimulateCashContributions(OptionsDict options)
			=> VehicleDefaultAS.SimulateCashContributionsDefaultPreTax(this, options);
	}
}
