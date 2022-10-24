using RetireSimple.Backend.DomainModel.Analysis.Strategy;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
    public class BondInvestment : InvestmentBase {
        public BondAS BondAS {
            get => default;
            set {
            }
        }


        public override InvestmentModel GenerateAnalysis() {
            throw new NotImplementedException();
        }
    }
}