using RetireSimple.Backend.DomainModel.Analysis.Strategy;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
    public abstract class StockInvestment : InvestmentBase {

        public double StockPrice { get; set; }

        public StockAS StockAS {
            get => default;
            set {
            }
        }

        public override InvestmentModel GenerateAnalysis() {
            throw new NotImplementedException();
        }

    }
}