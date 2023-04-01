namespace RetireSimple.Tests {
	public class AnalysisOptionCombinerTests {
		public static readonly OptionsDict dict = new() { { "test", "result" } };

		[Fact]
		public void TestStockASOptionsMerge() {
			var investment = new StockInvestment("");
			var investmentDict = new OptionsDict {
				{ "investment", "result" },
				{"test", "result2" }
			};
			investment.AnalysisOptionsOverrides = investmentDict;

			var newDict = StockAS.MergeAnalysisOptions(investment, dict);

			var expected = new OptionsDict() {
				["investment"] = "result",
				["test"] = "result",
				["AnalysisLength"] = "60",
				["RandomVariableMu"] = "0",
				["RandomVariableSigma"] = "1",
				["RandomVariableScaleFactor"] = "1",
				["SimCount"] = "1000"
			};

			newDict.Should().BeEquivalentTo(expected);
		}

		[Fact(Skip = "Investment Analysis Not Implemented Yet")]
		public void TestSocialSecurityAS() {
			//var newDict = SocialSecurityAS.MergeAnalysisOptions(investment, dict);
			//finalDict.Add("SocialSecurityAnalysisExpectedGrowth", "0.1");
			//Assert.Equal(newDict, finalDict);
		}

		[Fact(Skip = "Investment Analysis Not Implemented Yet")]
		public void TestPensionAS() {
			//var newDict = PensionAS.MergeAnalysisOption(investment, dict);
			//finalDict.Add("PensionAnalysisExpectedGrowth", "0.1");
			//Assert.Equal(newDict, finalDict);
		}

		[Fact(Skip = "Investment Analysis Not Implemented Yet")]
		public void TestFixedAS() {
			//var newDict = FixedAS.MergeAnalysisOptions(investment, dict);
			//finalDict.Add("FixedAnalysisExpectedGrowth", "0.1");
			//Assert.Equal(newDict, finalDict);
		}

		[Fact(Skip = "Investment Analysis Not Implemented Yet")]
		public void TestCashAS() {
			//var newDict = CashAS.MergeAnalysisOptions(investment, dict);
			//finalDict.Add("CashAnalysisExpectedGrowth", "0.1");
			//Assert.Equal(newDict, finalDict);
		}

		[Fact(Skip = "Investment Analysis Not Implemented Yet")]
		public void TestBondAS() {
			//var newDict = BondAS.MergeAnalysisOption(investment, dict);
			//finalDict.Add("BondAnalysisExpectedGrowth", "0.1");
			//Assert.Equal(newDict, finalDict);
		}

		[Fact(Skip = "Investment Analysis Not Implemented Yet")]
		public void TestAnnuityAS() {
			//var newDict = AnnuityAS.MergeAnalysisOption(investment, dict);
			//finalDict.Add("AnnuityAnalysisExpectedGrowth", "0.1");
			//Assert.Equal(newDict, finalDict);
		}

	}
}

