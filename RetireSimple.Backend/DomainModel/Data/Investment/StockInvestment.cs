using RetireSimple.Backend.DomainModel.Analysis.Strategy;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
    public abstract class StockInvestment : InvestmentBase {
        public StockAS StockAS {
            get => default;
            set {
            }
        }

        public override InvestmentModel generateAnalysis() {
            throw new NotImplementedException();
        }
    }
}