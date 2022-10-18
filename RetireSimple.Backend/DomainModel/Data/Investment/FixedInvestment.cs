using RetireSimple.Backend.DomainModel.Analysis.Strategy;

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