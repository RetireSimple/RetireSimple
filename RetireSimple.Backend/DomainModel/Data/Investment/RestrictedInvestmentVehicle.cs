namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public class RestrictedInvestmentVehicle : InvestmentBase {

		AnalysisDelegate<RestrictedInvestmentVehicle>? analysis;

		public override InvestmentModel InvokeAnalysis() => throw new NotImplementedException();
		public override void ResolveAnalysisDelegate(string analysisType) => throw new NotImplementedException();
	}
}
