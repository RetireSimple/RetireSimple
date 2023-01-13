using MathNet.Numerics.Distributions;

using Microsoft.Extensions.ObjectPool;

using RetireSimple.Backend.DomainModel.Analysis;
using RetireSimple.Backend.DomainModel.Data.Investment;

using System.Security.Cryptography.X509Certificates;

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
                new object[] { 0, 1, 1 },
                new object[] { 1, 0, 1 },
                new object[] { 1, 1, 1 },
                new object[] { 0.25, 5, 1 },
                new object[] { -0.25, 5, 1 },
                new object[] { 0, 1, 2 },
                new object[] { 1, 0, 2 },
                new object[] { 1, 1, 2 },
                new object[] { 0.25, 5, 2 },
                new object[] { -0.25, 5, 2 },
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
            NormalDistParams.Where(x => (double)x[0] >= 0);

        [Theory, MemberData(nameof(NormalDistParams))]
        public void TestSimulationTolerance_LogNormal(double mu, double sigma, double scaleFactor) {
            var options = new MonteCarloOptions() {
                BasePrice = TestInvestment.StockPrice,
                AnalysisLength = 60,
                RandomVarScaleFactor = (decimal)scaleFactor,
                RandomVariable = new LogNormal(mu, sigma)
            };

            var simResult = MonteCarloSim_SingleIteration(options);
            for (int i = 0; i < 59; i++) {
                var delta = (double)(simResult[i + 1] - simResult[i]);
                delta.Should().BeInRange(scaleFactor * Math.Exp(mu - (4 * sigma)),
                    scaleFactor * Math.Exp(mu + (4 * sigma)));
            }
        }




    }

}
