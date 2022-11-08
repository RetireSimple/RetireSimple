namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class PensionInvestment : InvestmentBase {
		public AnalysisDelegate<PensionInvestment>? analysis;

		public override InvestmentModel InvokeAnalysis(OptionsDict options) => throw new NotImplementedException();
		public override void ResolveAnalysisDelegate(string analysisType) => throw new NotImplementedException();
	}
}