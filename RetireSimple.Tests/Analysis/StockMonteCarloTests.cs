using MathNet.Numerics.Distributions;

using RetireSimple.Backend.DomainModel.Analysis;
using RetireSimple.Backend.DomainModel.Data.Investment;

using System.Security.Cryptography.X509Certificates;

using static RetireSimple.Backend.DomainModel.Analysis.MonteCarlo;

namespace RetireSimple.Tests.Analysis {
    public class StockMonteCarloTests {

        public StockInvestment TestInvestment { get; set; }

        //Array Structure
        // [mu, sigma, scale factor]
        public static readonly IEnumerable<object[]> distParams =
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

        public StockMonteCarloTests() {
            TestInvestment = new StockInvestment("test") {
                StockPrice = 100,
            };
        }

        [Theory, MemberData(nameof(distParams))]
        public void TestSimulationTolerance_NormalDistribution(double mu, double sigma, double scaleFactor) {
            //Methodology ideas

            //Run a simulation
            //For each step of a simulation:
            //  Assert that the delta between steps is within at most 3-4 std.
            //  devs of the mean

            //Statistically speaking, this should be true 99.7% of the time, given
            //props of the Normal Dist

            //Create SimOptions 
            var options = new MonteCarloOptions() {
                BasePrice = TestInvestment.StockPrice,
                AnalysisLength = 60,
                RandomVarScaleFactor = (decimal)scaleFactor,
                RandomVariable = new Normal(mu, sigma)
            };



            //Generate a Sim Reult
            var simResult = MonteCarlo.MonteCarloSim_SingleIteration(options);

            for (int i = 0; i < 59; i++) {
                var delta = simResult[i + 1] - simResult[i];
                Assert.True((double)delta <= (scaleFactor * (mu + (4 * sigma))));
                Assert.True((double)delta >= (scaleFactor * (mu - (4 * sigma))));
            }

        }


    }

}
