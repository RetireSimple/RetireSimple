using RetireSimple.Backend.DomainModel.Analysis.Strategy;
using RetireSimple.Backend.DomainModel.Data;

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