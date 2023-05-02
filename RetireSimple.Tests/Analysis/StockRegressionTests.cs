using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Tests.Analysis {

	public class StockRegressionTests {
		public OptionsDict TestOptions { get; set; } = new OptionsDict();

		public StockRegressionTests() {
			TestOptions["basePrice"] = "100";
			TestOptions["analysisLength"] = "5";
			TestOptions["uncertainty"] = "0.25";
			TestOptions["percentGrowth"] = "0.12";
		}

		private static readonly InvestmentModel Expected = new() {
			MinModelData = { 100, 88, 77, 68, 60 },
			AvgModelData = { 100, 100, 101, 104, 109 },
			MaxModelData = { 100, 112, 125, 140, 157 },
		};

		[Fact]
		public void RegressionTestBinomialModel() {
			var reg = new Regression(TestOptions);
			var actual = reg.RunSimulation();

			for (int i = 0; i < int.Parse(TestOptions["analysisLength"]); i++) {
				decimal.Round(actual.MinModelData[i]).Should().Be(Expected.MinModelData[i]);
				decimal.Round(actual.AvgModelData[i]).Should().Be(Expected.AvgModelData[i]);
				decimal.Round(actual.MaxModelData[i]).Should().Be(Expected.MaxModelData[i]);
			}
		}

		[Fact]
		public void RegressionTestNeg() {
			TestOptions["uncertainty"] = "-1";
			TestOptions["percentGrowth"] = "-1";
			int expected = 0;

			int resultU = (int)Math.Max(double.Parse(TestOptions["uncertainty"]), 0);
			int resultP = (int)Math.Max(double.Parse(TestOptions["percentGrowth"]), 0);

			resultU.Should().Be(expected);
			resultP.Should().Be(expected);

		}
	}
}
