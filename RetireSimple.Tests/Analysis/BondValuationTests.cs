namespace RetireSimple.Tests.Analysis {
	public class BondValuationTests {

		BondInvestment TestInvestment { get; set; }

		public static readonly OptionsDict DefaultBondAnalysisOptions = new() {
			["analysisLength"] = "60",
			["isAnnual"] = "true",
		};

		public BondValuationTests() {
			TestInvestment = new BondInvestment("") {
				BondCouponRate = 0.10,
				BondCurrentPrice = 0,
				BondFaceValue = 1000,
				AnalysisOptionsOverrides = new OptionsDict() {
					["analysisLength"] = "60",
					["isAnnual"] = "true",
				},
				BondYTM = 0.08M,
				BondPurchaseDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
				BondMaturityDate = new DateOnly(DateTime.Now.Year + 3, DateTime.Now.Month, DateTime.Now.Day)
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
		public void TestBondMatured() {
			TestInvestment.BondPurchaseDate = new DateOnly(DateTime.Now.Year - 4, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.BondMaturityDate = new DateOnly(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);

			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 4600M };

			Assert.Equal(ListOfBondVal[0], actual[0]);
			Assert.Equal(ListOfBondVal[59].ToString("#.##"), actual[0].ToString());
		}

		[Fact]
		public void TestBondAnnualCurrentEqualPurchase() {
			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 0M, 92.59M, 178.33M, 1051.54M };

			Assert.Equal(ListOfBondVal[0], actual[0]);
			Assert.Equal(ListOfBondVal[10], actual[0]);
			Assert.Equal(ListOfBondVal[11].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[22].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[23].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[34].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[35].ToString("#.##"), actual[3].ToString());
		}

		[Fact]
		public void TestBondAnnualCurrentAfterPurchase() {
			TestInvestment.BondPurchaseDate = new DateOnly(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.BondMaturityDate = new DateOnly(DateTime.Now.Year + 2, DateTime.Now.Month, DateTime.Now.Day);

			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 92.59M, 178.33M, 1051.54M };
			Assert.Equal(ListOfBondVal[0].ToString("#.##"), actual[0].ToString());
			Assert.Equal(ListOfBondVal[11].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[23].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[35].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[59].ToString("#.##"), actual[2].ToString());
		}

		[Fact]
		public void TestBondAnnualCurrentBeforePurchase() {
			TestInvestment.BondPurchaseDate = new DateOnly(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.BondMaturityDate = new DateOnly(DateTime.Now.Year + 4, DateTime.Now.Month, DateTime.Now.Day);

			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 0M, 92.59M, 178.33M, 1051.54M };
			Assert.Equal(ListOfBondVal[0], actual[0]);
			Assert.Equal(ListOfBondVal[11], actual[0]);
			Assert.Equal(ListOfBondVal[22], actual[0]);
			Assert.Equal(ListOfBondVal[23].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[34].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[35].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[46].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[47].ToString("#.##"), actual[3].ToString());
			Assert.Equal(ListOfBondVal[59].ToString("#.##"), actual[3].ToString());
		}



		[Fact]
		public void TestBondSemiannualCurrentEqualPurchase() {
			TestInvestment.AnalysisOptionsOverrides["isAnnual"] = "false";
			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 0, 92.59M, 178.33M, 257.71M, 331.21M, 399.27M, 1092.46M };
			Assert.Equal(ListOfBondVal[0], actual[0]);
			Assert.Equal(ListOfBondVal[4], actual[0]);
			Assert.Equal(ListOfBondVal[9].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[10].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[11].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[16].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[17].ToString("#.##"), actual[3].ToString());
			Assert.Equal(ListOfBondVal[22].ToString("#.##"), actual[3].ToString());
			Assert.Equal(ListOfBondVal[23].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[28].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[29].ToString("#.##"), actual[5].ToString());
			Assert.Equal(ListOfBondVal[34].ToString("#.##"), actual[5].ToString());
			Assert.Equal(ListOfBondVal[35].ToString("#.##"), actual[6].ToString());
		}

		[Fact]
		public void TestBondSemiAnnualCurrentAfterPurchase() {
			TestInvestment.BondPurchaseDate = new DateOnly(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.BondMaturityDate = new DateOnly(DateTime.Now.Year + 2, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.AnalysisOptionsOverrides["isAnnual"] = "false";

			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 178.33M, 257.71M, 331.21M, 399.27M, 1092.46M };
			Assert.Equal(ListOfBondVal[0].ToString("#.##"), actual[0].ToString());
			Assert.Equal(ListOfBondVal[5].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[11].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[17].ToString("#.##"), actual[3].ToString());
			Assert.Equal(ListOfBondVal[23].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[29].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[35].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[41].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[47].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[53].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[59].ToString("#.##"), actual[4].ToString());
		}

		[Fact]
		public void TestBondSemiAnnualCurrentBeforePurchase() {
			TestInvestment.BondPurchaseDate = new DateOnly(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.BondMaturityDate = new DateOnly(DateTime.Now.Year + 4, DateTime.Now.Month, DateTime.Now.Day);
			TestInvestment.AnalysisOptionsOverrides["isAnnual"] = "false";
			var ListOfBondVal = BondAS.BondValuation(TestInvestment, DefaultBondAnalysisOptions);
			decimal[] actual = { 0, 92.59M, 178.33M, 257.71M, 331.21M, 399.27M, 1092.46M };
			Assert.Equal(ListOfBondVal[0], actual[0]);
			Assert.Equal(ListOfBondVal[5], actual[0]);
			Assert.Equal(ListOfBondVal[11], actual[0]);
			Assert.Equal(ListOfBondVal[17].ToString("#.##"), actual[1].ToString());
			Assert.Equal(ListOfBondVal[23].ToString("#.##"), actual[2].ToString());
			Assert.Equal(ListOfBondVal[29].ToString("#.##"), actual[3].ToString());
			Assert.Equal(ListOfBondVal[35].ToString("#.##"), actual[4].ToString());
			Assert.Equal(ListOfBondVal[41].ToString("#.##"), actual[5].ToString());
			Assert.Equal(ListOfBondVal[47].ToString("#.##"), actual[6].ToString());
			Assert.Equal(ListOfBondVal[53].ToString("#.##"), actual[6].ToString());
			Assert.Equal(ListOfBondVal[59].ToString("#.##"), actual[6].ToString());
		}
	}
}