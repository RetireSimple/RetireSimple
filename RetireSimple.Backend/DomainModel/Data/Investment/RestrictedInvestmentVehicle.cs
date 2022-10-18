using RetireSimple.Backend.DomainModel.Analysis.Strategy;
using RetireSimple.Backend.DomainModel.Data;

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
