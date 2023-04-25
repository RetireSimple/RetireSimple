using MathNet.Numerics.Distributions;

using Moq;

using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data.Analysis;

using System.Collections.Concurrent;

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

		[Fact]
		public void RunSimulation_SingleSimulation_ReturnsExpectedData() {
			var dummyRV = new Mock<IContinuousDistribution>();
			dummyRV.Setup(x => x.Sample()).Returns(1);
			var monteCarlo = new MonteCarlo(TestOptions);


			var actual = new List<decimal>();
			monteCarlo.MonteCarloSingleSimulation(dummyRV.Object, ref actual);
			actual.Should().BeEquivalentTo(new List<decimal> { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 });
		}

		delegate void MonteCarloSingleSimulationCallback(IContinuousDistribution rv, ref List<decimal> list);

		[Fact]
		public void RunFullSimulation_PerformsExpectedSteps() {
			//While we can't exactly or effectively predict probability, we can
			//verify the steps performed by the simulation are correct

			var dummyRV = new Mock<IContinuousDistribution>();
			dummyRV.Setup(x => x.Sample()).Returns(1);
			var monteCarlo = new Mock<MonteCarlo>(TestOptions) { CallBase = true };
			monteCarlo.Setup(x => x.MonteCarloSingleSimulation(It.IsAny<IContinuousDistribution>(), ref It.Ref<List<decimal>>.IsAny))
			.Callback(new MonteCarloSingleSimulationCallback((IContinuousDistribution rv, ref List<decimal> list)
															=> list.AddRange(Enumerable.Range(0, 10).Select(x => 100m + x))));

			var actual = monteCarlo.Object.RunSimulation();

			actual.MinModelData.Should().HaveCount(10);
			actual.MaxModelData.Should().HaveCount(10);
			actual.AvgModelData.Should().HaveCount(10);

			monteCarlo.Verify(x => x.MonteCarloSingleSimulation(It.IsAny<IContinuousDistribution>(), ref It.Ref<List<decimal>>.IsAny), Times.AtLeast(1000));  //Accounts for rounding from threading
		}
	}
}
