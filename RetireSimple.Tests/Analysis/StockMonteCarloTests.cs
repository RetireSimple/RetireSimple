using RetireSimple.Backend.DomainModel.Data.Investment;

using System.Security.Cryptography.X509Certificates;

namespace RetireSimple.Tests.Analysis {
    public class StockMonteCarloTests {

        public StockInvestment TestInvestment { get; set; }

        //Array Structure
        // [mu, sigma, scale factor]
        public static readonly IEnumerable<object[]> distParams =
            new List<object[]> { new object[] { 0, 1, 1 },
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

            var options = new OptionsDict() {
                ["RandomVarMu"] = mu.ToString(),
                ["RandomVarSigma"] = sigma.ToString(),
                ["RandomVarScaleFactor"] = scaleFactor.ToString(),
                ["RandomVarType"] = "Normal",

            };

            
        }
    }
}
