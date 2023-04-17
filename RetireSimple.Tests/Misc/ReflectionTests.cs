using RetireSimple.Engine.Analysis.Presets;
using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Tests.Misc {
	public class ReflectionTests {

#pragma warning disable CS8604 //We expect these to resolve under normal scenarios
		public static readonly IEnumerable<object[]> ReflectionTheoryData = new List<object[]>() {
			new object[]{
				new StockInvestment(""),
				Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(StockInvestment)),
										typeof(StockAS).GetMethod("MonteCarlo"))
			},
			new object[]{
				new BondInvestment(""),
				Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(BondInvestment)),
										typeof(BondAS).GetMethod("StdBondValuation"))
			},
		};
#pragma warning restore CS8604

		[Theory, MemberData(nameof(ReflectionTheoryData))]
		public void SetAnalysisMethodDelegate_ResolvesCorrectDelegate(Investment investment, Delegate expectedDelegate) {
			//The purpose of this test is to ensure we can set a delegate via reflection
			//Check if we have a clean slate first (i.e. AnalysisMethod is null)
			var analysisMethodProp = investment.GetType().GetProperty("AnalysisMethod");
			analysisMethodProp.Should().NotBeNull();
			analysisMethodProp?.GetValue(investment).Should().BeNull();
			var type = investment.GetType();
			var delegateType = typeof(AnalysisModule<>).MakeGenericType(type);

			var genericSetMethod = typeof(ReflectionUtils).GetMethod("SetAnalysisModuleDelegate")?.MakeGenericMethod(type);

			genericSetMethod?.Invoke(null, new object[] { investment, expectedDelegate });

			analysisMethodProp?.GetValue(investment).Should().NotBeNull();
			analysisMethodProp?.GetValue(investment).Should().BeOfType(delegateType);
		}

#pragma warning disable CS8604
		public static readonly IEnumerable<object[]> ExpectedAnalysisModuleResolution = new List<object[]> {
			new object[] {"StockInvestment", new Dictionary<string,Delegate>(){
				["MonteCarlo"] = Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(StockInvestment)),
																	typeof(StockAS).GetMethod("MonteCarlo"))}
			},
			new object[] {"BondInvestment", new Dictionary<string,Delegate>(){
				["StdBondValuation"] = Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(BondInvestment)),
																typeof(BondAS).GetMethod("StdBondValuation"))
				}
			}
		};
#pragma warning restore CS8604

		[Theory, MemberData(nameof(ExpectedAnalysisModuleResolution))]
		public void GetAnalysisModules_DoesResolveMinimumExpectations(string module, Dictionary<string, Delegate> expectedDict) {
			var actualDict = ReflectionUtils.GetAnalysisModules(module);

			actualDict.Should().NotBeNull();
			actualDict.Should().HaveCountGreaterThanOrEqualTo(expectedDict.Count);
			//Becuase we can't necessarily generate equality for the Delegate type, compare by common values we expect
			foreach (var key in expectedDict.Keys) {
				actualDict.Should().ContainKey(key);
				actualDict[key].Should().NotBeNull();
				actualDict[key].Should().BeOfType(expectedDict[key].GetType());
				actualDict[key].Method.Name.Should().Be(expectedDict[key].Method.Name);
				actualDict[key].Method.DeclaringType.Should().Be(expectedDict[key].Method.DeclaringType);
			}
		}

		[Fact]
		public void GetInvestmentModules_ReturnsKnownCorrectModules() {
			var moduleList = ReflectionUtils.GetInvestmentModules();

			moduleList.Should().NotBeNull();
			moduleList.Should().HaveCountGreaterThanOrEqualTo(2);
			moduleList.Should().Contain(typeof(StockInvestment));
			moduleList.Should().Contain(typeof(BondInvestment));
		}

		[Fact]
		public void GetInvestmentVehicleModules_ReturnsKnownCorrectModules() {
			var moduleList = ReflectionUtils.GetInvestmentVehicleModules();

			moduleList.Should().NotBeNull();
			moduleList.Should().HaveCountGreaterThanOrEqualTo(5);
			moduleList.Should().Contain(typeof(Vehicle401k));
			moduleList.Should().Contain(typeof(Vehicle403b));
			moduleList.Should().Contain(typeof(Vehicle457));
			moduleList.Should().Contain(typeof(VehicleIRA));
			moduleList.Should().Contain(typeof(VehicleRothIRA));
		}

		[Fact]
		public void GetAnalysisPresets_GivenAnalysisNameWithPresets_CorrectlyReturnsPresets() {
			var presetList = ReflectionUtils.GetAnalysisPresets("MonteCarlo");

			presetList.Should().NotBeNull();
			presetList.Should().HaveCountGreaterThanOrEqualTo(7);
			presetList.Should().ContainKey("DefaultStockAnalysis");
			presetList["DefaultStockAnalysis"].Should().NotBeNull();
			presetList["DefaultStockAnalysis"].Should().BeEquivalentTo(MonteCarloPresets.DefaultStockAnalysis);

			presetList.Should().ContainKey("LargeCapGrowth");
			presetList["LargeCapGrowth"].Should().NotBeNull();
			presetList["LargeCapGrowth"].Should().BeEquivalentTo(MonteCarloPresets.LargeCapGrowth);

			presetList.Should().ContainKey("LargeCapValue");
			presetList["LargeCapValue"].Should().NotBeNull();
			presetList["LargeCapValue"].Should().BeEquivalentTo(MonteCarloPresets.LargeCapValue);

			presetList.Should().ContainKey("MidCapGrowth");
			presetList["MidCapGrowth"].Should().NotBeNull();
			presetList["MidCapGrowth"].Should().BeEquivalentTo(MonteCarloPresets.MidCapGrowth);

			presetList.Should().ContainKey("MidCapValue");
			presetList["MidCapValue"].Should().NotBeNull();
			presetList["MidCapValue"].Should().BeEquivalentTo(MonteCarloPresets.MidCapValue);

			presetList.Should().ContainKey("SmallCapGrowth");
			presetList["SmallCapGrowth"].Should().NotBeNull();
			presetList["SmallCapGrowth"].Should().BeEquivalentTo(MonteCarloPresets.SmallCapGrowth);

			presetList.Should().ContainKey("SmallCapValue");
			presetList["SmallCapValue"].Should().NotBeNull();
			presetList["SmallCapValue"].Should().BeEquivalentTo(MonteCarloPresets.SmallCapValue);
		}

		[Fact]
		public void GetAnalysisPresets_GivenAnalysisNameWithNoPresets_ReturnsEmpty() {
			var presetList = ReflectionUtils.GetAnalysisPresets("StdBondValuation");

			presetList.Should().NotBeNull();
			presetList.Should().BeEmpty();
		}
	}
}