namespace RetireSimple.Backend.DomainModel.Analysis.Strategy {
    public abstract class PensionAS : IAnalysisStrategy {
        public Data.Investment.PensionInvestment PensionInvestment {
            get => default;
            set {
            }
        }
    }
}