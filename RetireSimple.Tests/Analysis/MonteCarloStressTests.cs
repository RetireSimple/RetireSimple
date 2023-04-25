using MathNet.Numerics.Distributions;

using RetireSimple.Engine.Analysis.Utils;

namespace RetireSimple.Tests.Analysis {
	public class MonteCarloStressTests {
		[Fact]
		public void MonteCarloStressTest() {
			var monteCarlo = new MonteCarlo(new OptionsDict() {
				["basePrice"] = "100",
				["analysisLength"] = "60",
				["randomVariableScaleFactor"] = "1",
				["simCount"] = "1000000",
				["randomVariableType"] = "Normal",
				["randomVariableMu"] = "2",
				["randomVariableSigma"] = "5",
			});

			var result = monteCarlo.RunSimulationImproved();

			result.AvgModelData.Should().HaveCount(60);
			result.MinModelData.Should().HaveCount(60);
			result.MaxModelData.Should().HaveCount(60);
		}

	}
}