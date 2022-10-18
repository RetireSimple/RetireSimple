namespace RetireSimple.Backend.DomainModel.Data {
    public abstract class InvestmentBase {
        int investmentId { get => investmentId; set => investmentId = value; }

        public abstract InvestmentModel generateAnalysis();
    }
}