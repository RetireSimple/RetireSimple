using FluentAssertions.Execution;

using MathNet.Numerics.Distributions;

using static RetireSimple.Engine.Analysis.MonteCarlo;

namespace RetireSimple.Tests.Analysis {
	public class StockMonteCarloTests {

		public StockInvestment TestInvestment { get; set; }

		public StockMonteCarloTests() {
			TestInvestment = new StockInvestment("") {
				StockPrice = 100,
			};
		}

		//Array Structure
		// [mu, sigma, scale factor]
		public static readonly IEnumerable<object[]> NormalDistParams =
			new List<object[]> {
				new object[] { 0d, 1d, 1d },
				new object[] { 1d, 0d, 1d },
				new object[] { 1d, 1d, 1d },
				new object[] { 0.25d, 5d, 1d },
				new object[] { -0.25d, 5d, 1d },
				new object[] { 0d, 1d, 2d },
				new object[] { 1d, 0d, 2d },
				new object[] { 1d, 1d, 2d },
				new object[] { 0.25d, 5d, 2d },
				new object[] { -0.25d, 5d, 2d },
			};
		[Theory, MemberData(nameof(NormalDistParams))]
		public void TestSimulationTolerance_Normal(double mu, double sigma, double scaleFactor) {
			var options = new MonteCarloOptions() {
				BasePrice = TestInvestment.StockPrice,
				AnalysisLength = 60,
				RandomVarScaleFactor = (decimal)scaleFactor,
				RandomVariable = new Normal(mu, sigma)
			};

			using (new AssertionScope("root")) {
				//Rerun test multiple times because of probability
				for (int i = 0; i < 100; i++) {
					var simResult = MonteCarloSimSingleIteration(options);
					for (int step = 0; step < 59; step++) {
						using (new AssertionScope($"iteration {i} - step {step}")) {
							var delta = (double)(simResult[step + 1] - simResult[step]);
							delta.Should().BeInRange(scaleFactor * (mu - (6 * sigma)),
								scaleFactor * (mu + (6 * sigma)));
						}
					}

				}
			}
		}

		public static readonly IEnumerable<object[]> LogNormalDistParams =
			NormalDistParams.Where(x => (double)(x[0]) >= 0);
		[Theory, MemberData(nameof(LogNormalDistParams))]
		public void TestSimulationTolerance_LogNormal(double mu, double sigma, double scaleFactor) {
			var options = new MonteCarloOptions() {
				BasePrice = TestInvestment.StockPrice,
				AnalysisLength = 60,
				RandomVarScaleFactor = (decimal)scaleFactor,
				RandomVariable = new LogNormal(mu, sigma)
			};

			using (new AssertionScope("root")) {
				//Rerun test multiple times because of probability
				for (int i = 0; i < 100; i++) {
					using (new AssertionScope($"iteration-{i}")) {
						//NOTE Minor rounding is used here to increase test
						var simResult = MonteCarloSimSingleIteration(options);
						for (int step = 0; step < 59; step++) {
							using (new AssertionScope($"iteration {i} - step {step}")) {
								var delta = (double)Math.Round((simResult[step + 1] - simResult[step]), 7);
								var minDeltaRange = Math.Round(scaleFactor * Math.Exp((mu - (6 * sigma))), 7);
								var maxDeltaRange = Math.Round(scaleFactor * Math.Exp((mu + (6 * sigma))), 7);

								delta.Should().BeInRange(minDeltaRange, maxDeltaRange);
							}
						}
					}
				}
			}

		}
	}
}
