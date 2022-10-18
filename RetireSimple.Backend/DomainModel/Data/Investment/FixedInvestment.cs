using RetireSimple.Backend.DomainModel.Analysis.Strategy;
using RetireSimple.Backend.DomainModel.Data;

namespace RetireSimple.Backend.DomainModel.Data.Investment {

    public abstract class FixedInvestment : InvestmentBase {

        public FixedAS FixedAS {
            get => default;
            set {
            }
        }

        public override InvestmentModel generateAnalysis() {
            throw new NotImplementedException();
        }
    }
}