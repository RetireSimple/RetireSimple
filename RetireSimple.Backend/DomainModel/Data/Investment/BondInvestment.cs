using RetireSimple.Backend.DomainModel.Analysis.Strategy;
using RetireSimple.Backend.DomainModel.Data;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
    public class BondInvestment : InvestmentBase {
        public BondAS BondAS {
            get => default;
            set {
            }
        }


        public override InvestmentModel generateAnalysis() {
            throw new NotImplementedException();
        }
    }
}