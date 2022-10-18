namespace RetireSimple.Backend.DomainModel.Analysis.Strategy {
    public abstract class StockAS : IAnalysisStrategy {
        public Data.Investment.StockInvestment StockInvestment {
            get => default;
            set {
            }
        }
    }
}