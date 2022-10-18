namespace RetireSimple.Backend.DomainModel.Analysis.Strategy {

    public abstract class FixedAS : IAnalysisStrategy {
        public Data.Investment.FixedInvestment FixedInvestment {
            get => default;
            set {
            }
        }
    }
}