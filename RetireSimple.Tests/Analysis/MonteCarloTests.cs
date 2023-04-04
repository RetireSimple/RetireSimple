using MathNet.Numerics.Distributions;

using RetireSimple.Engine.Analysis.Utils;

namespace RetireSimple.Tests.Analysis {
	public class MonteCarloTests {

		public OptionsDict TestOptions { get; set; } = new OptionsDict();

		public MonteCarloTests() {
			TestOptions["basePrice"] = "100";
			TestOptions["analysisLength"] = "10";
			TestOptions[""]
		}

		public static readonly IEnumerable<object[]> RandomVariableCreationData = new List<object[]>{
			new object[]{MonteCarloRV.Normal, new OptionsDict(){{"mu", "0"}, {"sigma", "1"}}, typeof(Normal)},
		};
		[Theory, MemberData(nameof(RandomVariableCreationData))]
		public void RandomVariableCreation_KnownType_ReturnsCorrectInstance(MonteCarloRV rvType, OptionsDict options, Type expectedType) {
			var monteCarloEngine = new MonteCarlo(new OptionsDict());
			var rv = monteCarloEngine.CreateRandomVarInstance(rvType, options);
			rv.Should().NotBeNull();
			rv.Should().BeOfType(expectedType);
		}

		[Fact]
		public void RandomVariableCreation_UnknownType_ThrowsException() {
			var monteCarloEngine = new MonteCarlo(TestOptions);
			Action act = () => monteCarloEngine.CreateRandomVarInstance(MonteCarloRV.Chi, new OptionsDict());
			act.Should().Throw<NotImplementedException>();
		}


	}
}
