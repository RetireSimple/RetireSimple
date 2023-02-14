namespace RetireSimple.Tests.Analysis {
	public class BondValuationTests {

		BondInvestment TestInvestment { get; set; }

		public BondValuationTests() {
			TestInvestment = new BondInvestment("") {
				BondCouponRate = 0.10,
				BondCurrentPrice = 0,
				BondFaceValue = 1000,
				BondIsAnnual = "Annual",
				BondYTM = 0.08M,
				BondPurchaseDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
				BondMaturityDate = new DateOnly(DateTime.Now.Year+3, DateTime.Now.Month, DateTime.Now.Day)
			};
		}

		//3 yr bond with 10% coupon rate and Rs 1,000 face val has yeild maturity of 8%	
		//Assuming annual coupon rate payment find price of bond
		// n = 3
		// coupon rate = 0.10
		// face val = 1000
		// disc rate = 0.08
		// coupon payment = coupon rate * face val = 1000 * 0.10 = 100
		//
		//	Time	0		1		2		3
		//	CFlow	0		100		100		100
		//							+ return initial 1000
		// Single Cash Flow Formula
		// present val = sum ( Future val / (1 + disc rate)^n )
		//	Time	0		1		2		3
		//	pv		0		92.59 +	85.73 +	873.21 = 1051.55	
		[Fact]
		public void TestBondCurrentValAnnual() {
			var ListOfBondVal = BondAS.BondValuation(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			decimal[] actual = {92.59M, 178.33M, 1051.54M};
			Assert.Equal(ListOfBondVal[0].ToString("#.##"), actual[0].ToString());
			Assert.Equal(ListOfBondVal[1].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[2].ToString("#.##"), actual[2].ToString());
		}
		[Fact]
		public void TestBondCurrentValSemiannual() {
			TestInvestment.BondIsAnnual = "SemiAnnual";
			var ListOfBondVal = BondAS.BondValuation(TestInvestment, BondAS.DefaultBondAnalysisOptions);
			decimal[] actual = {92.59M, 178.33M, 257.71M, 331.21M, 399.27M, 1092.46M};
			Assert.Equal(ListOfBondVal[0].ToString("#.##"), actual[0].ToString());
			Assert.Equal(ListOfBondVal[1].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[2].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[3].ToString("#.##"), actual[3].ToString());
			Assert.Equal(ListOfBondVal[4].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[5].ToString("#.##"), actual[5].ToString());
		}
	}
}