namespace RetireSimple.Tests.Analysis {
	public class BondValuationTests {

		BondInvestment TestInvestment { get; set; }

		private readonly ITestOutputHelper output;


		public BondValuationTests(ITestOutputHelper output) {

			TestInvestment = new BondInvestment("");
			TestInvestment.BondCouponRate = 0.03;
			TestInvestment.BondCurrentPrice = 0;
			TestInvestment.BondPurchasePrice = 1000;
			TestInvestment.BondIsAnnual = true;
            TestInvestment.BondPurchaseDate = new DateOnly(2003, 2, 7);
            TestInvestment.BondMaturityDate = new DateOnly(2033, 2, 7);
			this.output = output;
		}

		public void TestBondPeriodicCashFlowAnnual() {
			var cashFlow = BondAS.periodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			Assert.Equal(30, cashFlow);
			
		}

        public void TestBondPeriodicCashFlowSemiannual()
        {
            var cashFlow = BondAS.periodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
            Assert.Equal(15, cashFlow);

        }

        public void TestBondCurrentValAnnual()
        {
            var cashFlow = BondAS.periodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
            Assert.Equal(1000, cashFlow);

        }
        public void TestBondCurrentValSemiannual()
        {
            var cashFlow = BondAS.periodicCashFlow(TestInvestment, BondAS.DefaultBondAnalysisOptions);
            Assert.Equal(1000, cashFlow);

        }
    }
}