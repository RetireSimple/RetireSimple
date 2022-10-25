using RetireSimple.Backend.DomainModel.Analysis;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public abstract class RestrictedInvestmentVehicle : InvestmentBase {


		public override void ValidateData() => throw new NotImplementedException();
	}
}
