﻿using RetireSimple.Engine.Analysis;

namespace RetireSimple.Engine.Data.InvestmentVehicle {
	public class Vehicle457 : InvestmentVehicleBase {
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
