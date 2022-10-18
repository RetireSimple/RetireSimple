using RetireSimple.Backend.DomainModel.Analysis.Strategy;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
    public class RestrictedInvestmentVehicle : InvestmentBase {
        public RestrictedInvestmentVehicleAS RestrictedInvestmentVehicleAS {
            get => default;
            set {
            }
        }

        public override InvestmentModel generateAnalysis() {
            throw new NotImplementedException();
        }
    }
}
