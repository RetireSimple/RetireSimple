using RetireSimple.Engine.Analysis;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	public class Vehicle401k : InvestmentVehicleBase {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePreTaxModelDefaultAfterTaxVehicle(options, models, cashContribution);
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePostTaxModelDefaultAfterTaxVehicle(options, models, cashContribution);
		public override List<decimal> SimulateCashContributions(OptionsDict options)
			=> VehicleDefaultAS.SimulateCashContributionsDefaultAfterTax(this, options);
	}
}
