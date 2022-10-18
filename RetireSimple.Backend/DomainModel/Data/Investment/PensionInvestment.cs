using RetireSimple.Backend.DomainModel.Analysis.Strategy;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
    public abstract class PensionInvestment : InvestmentBase {

        public PensionAS PensionAS {
            get => default;
            set {
            }
        }

        public override InvestmentModel generateAnalysis() {
            throw new NotImplementedException();
        }
    }
}