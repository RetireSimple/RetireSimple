namespace RetireSimple.Tests.Analysis {
	public class BondValuationTests {

		BondInvestment TestInvestment { get; set; }

		public BondValuationTests() {

			TestInvestment = new BondInvestment("") {
				BondCouponRate = 0.03,
				BondCurrentPrice = 0,
				BondPurchasePrice = 1000,
				BondIsAnnual = true,
				BondPurchaseDate = new DateOnly(2003, 2, 7),
				BondMaturityDate = new DateOnly(2033, 2, 7)
			};
		}

		public void TestBondPeriodicCashFlowAnnual() {
			var cashFlow = BondAS.PeriodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			Assert.Equal(30, cashFlow);

		}

		public void TestBondPeriodicCashFlowSemiannual() {
			var cashFlow = BondAS.PeriodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			Assert.Equal(15, cashFlow);

		}

		public void TestBondCurrentValAnnual() {
			var cashFlow = BondAS.PeriodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			Assert.Equal(1000, cashFlow);

		}
		public void TestBondCurrentValSemiannual() {
			var cashFlow = BondAS.PeriodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			Assert.Equal(1000, cashFlow);

		}
	}
}