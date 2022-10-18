namespace RetireSimple.Backend.DomainModel.Analysis.Strategy {

    public abstract class BondAS : IAnalysisStrategy {
        public Data.Investment.BondInvestment BondInvestment {
            get => default;
            set {
            }
        }
    }
}