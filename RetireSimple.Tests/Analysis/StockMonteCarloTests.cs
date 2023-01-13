using MathNet.Numerics.Distributions;

using RetireSimple.Backend.DomainModel.Data.Investment;

using static RetireSimple.Backend.DomainModel.Analysis.MonteCarlo;

namespace RetireSimple.Tests.Analysis {
    public class StockMonteCarloTests {

        public StockInvestment TestInvestment { get; set; }

        public StockMonteCarloTests() {
            TestInvestment = new StockInvestment("test") {
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

            var simResult = MonteCarloSim_SingleIteration(options);
            for (int i = 0; i < 59; i++) {
                var delta = (double)(simResult[i + 1] - simResult[i]);
                delta.Should().BeInRange(scaleFactor * (mu - (4 * sigma)),
                    scaleFactor * (mu + (4 * sigma)));
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

            //NOTE Minor rounding is used here to increase test determinism
            var simResult = MonteCarloSim_SingleIteration(options);
            for (int i = 0; i < 59; i++) {
                var delta = (double)Math.Round((simResult[i + 1] - simResult[i]), 7);
                var minDeltaRange = Math.Round(scaleFactor * Math.Exp((mu - (4 * sigma))), 7);
                var maxDeltaRange = Math.Round(scaleFactor * Math.Exp((mu + (4 * sigma))), 7);

                delta.Should().BeInRange(minDeltaRange, maxDeltaRange);
            }
        }

    }

}
