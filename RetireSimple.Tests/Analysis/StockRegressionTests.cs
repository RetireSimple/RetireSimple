using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data.Analysis;

namespace RetireSimple.Tests.Analysis {

	public class StockRegressionTests {

		private readonly ITestOutputHelper output;

		public OptionsDict TestOptions { get; set; } = new OptionsDict();

		public StockRegressionTests(ITestOutputHelper output) {
			TestOptions["stockQuantity"] = "5";
			TestOptions["basePrice"] = "20";
			TestOptions["analysisLength"] = "5";
			TestOptions["uncertainty"] = "0.25";
			TestOptions["percentGrowth"] = "0.12";
			this.output = output;
		}

		InvestmentModel expected = new InvestmentModel() {
			MinModelData = { 100, 88, 77, 68, 60 },
			AvgModelData = { 100, 100, 101, 104, 109 },
			MaxModelData = { 100, 112, 125, 140, 157 },
		};

		[Fact]
		public void RegressionTestBinomialModel() {
			var reg = new Regression(TestOptions);
			var actual = reg.RunSimulation();
			for (var i = 0; i < int.Parse(TestOptions["analysisLength"]); i++) {
				output.WriteLine($"Min: {actual.MinModelData[i]}");
				output.WriteLine($"Avg: {actual.AvgModelData[i]}");
				output.WriteLine($"Max: {actual.MaxModelData[i]}");
			}

			for (int i = 0; i < int.Parse(TestOptions["analysisLength"]); i++) {
				decimal.Round(actual.MinModelData[i]).Should().Be(expected.MinModelData[i]);
				decimal.Round(actual.AvgModelData[i]).Should().Be(expected.AvgModelData[i]);
				decimal.Round(actual.MaxModelData[i]).Should().Be(expected.MaxModelData[i]);
			}
		}

		[Fact]
		public void RegressionTestNeg() {
			TestOptions["uncertainty"] = "-1";
			TestOptions["percentGrowth"] = "-1";
			int expected = 0;

			int resultU = (int)Math.Max(double.Parse(TestOptions["uncertainty"]), 0);
			int resultP = (int)Math.Max(double.Parse(TestOptions["percentGrowth"]), 0);

			Assert.Equal(expected, resultU);
		}
	}
}
