namespace RetireSimple.Backend.DomainModel.Data.InvestmentVehicle {
	public class Vehicle403b : InvestmentVehicleBase {
		public override InvestmentModel GeneratePostTaxModels(OptionsDict options, List<InvestmentModel> models, List<decimal>? cashContribution = null) => throw new NotImplementedException();
		public override InvestmentModel GeneratePreTaxModels(OptionsDict options, List<InvestmentModel> models, List<decimal>? cashContribution = null) => throw new NotImplementedException();
		public override List<decimal> SimulateCashContributions(OptionsDict options) => throw new NotImplementedException();
	}
}
