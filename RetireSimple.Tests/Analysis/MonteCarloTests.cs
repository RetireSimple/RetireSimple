using MathNet.Numerics.Distributions;

using RetireSimple.Engine.Analysis.Utils;

namespace RetireSimple.Tests.Analysis {
	public class MonteCarloTests {

		public OptionsDict TestOptions { get; set; } = new OptionsDict();

		public MonteCarloTests() {
			TestOptions["basePrice"] = "100";
			TestOptions["analysisLength"] = "10";
			TestOptions["randomVariableType"] = "Normal";
			TestOptions["randomVariableMu"] = "0.1";
			TestOptions["randomVariableSigma"] = "0.2";
			TestOptions["randomVariableScaleFactor"] = "1";
			TestOptions["simCount"] = "1000";
		}

		public static readonly IEnumerable<object[]> RandomVariableCreationData = new List<object[]>{
			new object[]{new OptionsDict(){{"randomVariableType", "Normal"}, {"randomVariableMu", "0"}, {"randomVariableSigma", "1"}}, typeof(Normal)},
			new object[]{new OptionsDict(){{"randomVariableType", "LogNormal" }, {"randomVariableMu", "0"}, {"randomVariableSigma", "1"}}, typeof(LogNormal)},
		};
		[Theory, MemberData(nameof(RandomVariableCreationData))]
		public void RandomVariableCreation_KnownType_ReturnsCorrectInstance(OptionsDict options, Type expectedType) {
			var rv = MonteCarlo.CreateRandomVariable(options);
			rv.Should().NotBeNull();
			rv.Should().BeOfType(expectedType);
		}

		[Fact]
		public void RandomVariableCreation_UnknownType_ThrowsException() {
			Action act = () => MonteCarlo.CreateRandomVariable(new OptionsDict() { { "randomVariableType", "Chi" } });
			act.Should().Throw<NotImplementedException>();
		}


	}
}
