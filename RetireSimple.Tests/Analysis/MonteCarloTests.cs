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
		public void FilterSimulationData_PerformsExpectedFilterning() {
			var data = new ConcurrentBag<List<decimal>>() {
				new List<decimal> {
					1, 2, 3, 4, 5, 6, 7, 8, 9, 10
				},
				new List<decimal>{10, 9, 8, 7, 6, 5, 4, 3, 2, 1},
			};
			var expected = new InvestmentModel() {
				MinModelData = { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 },
				MaxModelData = { 10, 9, 8, 7, 6, 6, 7, 8, 9, 10 },
				AvgModelData = { 5.5M, 5.5M, 5.5M, 5.5M, 5.5M, 5.5M, 5.5M, 5.5M, 5.5M, 5.5M },
			};
			var dummyRV = new Mock<IContinuousDistribution>();


			var monteCarlo = new MonteCarlo(TestOptions, dummyRV.Object);

			var actual = monteCarlo.FilterSimulationData(data, 10);
			//Quickly set LastUpdated to prevent equality errors
			var now = DateTime.Now;
			expected.LastUpdated = now;
			actual.LastUpdated = now;

			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void RunSimulation_SingleSimulation_ReturnsExpectedData() {
			var dummyRV = new Mock<IContinuousDistribution>();
			dummyRV.Setup(x => x.Sample()).Returns(1);
			var monteCarlo = new MonteCarlo(TestOptions, dummyRV.Object);
			var actual = monteCarlo.MonteCarloSingleSimulation();
			actual.Should().BeEquivalentTo(new List<decimal> { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 });
		}

		[Fact]
		public void RunFullSimulation_PerformsExpectedSteps() {
			//While we can't exactly or effectively predict probability, we can 
			//verify the steps performed by the simulation are correct

			var dummyRV = new Mock<IContinuousDistribution>();
			dummyRV.Setup(x => x.Sample()).Returns(1);
			var monteCarlo = new Mock<MonteCarlo>(TestOptions, dummyRV.Object);
			monteCarlo.Setup(x => x.MonteCarloSingleSimulation())
						.Returns(new List<decimal>());
			monteCarlo.Setup(x => x.FilterSimulationData(It.IsAny<ConcurrentBag<List<decimal>>>(), It.IsAny<int>()))
						.Returns(new InvestmentModel());

			var actual = monteCarlo.Object.RunSimulation();
			monteCarlo.Verify(x => x.MonteCarloSingleSimulation(), Times.Exactly(1000));
			monteCarlo.Verify(x => x.FilterSimulationData(It.IsAny<ConcurrentBag<List<decimal>>>(), It.IsAny<int>()), Times.Once);
		}
	}
}
