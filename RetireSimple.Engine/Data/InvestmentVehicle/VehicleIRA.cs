using RetireSimple.Engine.Analysis;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	[InvestmentVehicleModule]
	public class VehicleIRA : Base.InvestmentVehicle {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePreTaxModelDefault(options, models, cashContribution);
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options,
															List<InvestmentModel> models,
															List<decimal>? cashContribution = null)
			=> VehicleDefaultAS.GeneratePostTaxModelDefault(options, models, cashContribution);
		public override List<decimal> SimulateCashContributions(OptionsDict options)
			=> VehicleDefaultAS.SimulateCashContributionsDefault(this, options);
	}
}
