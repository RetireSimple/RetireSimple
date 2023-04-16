using RetireSimple.Engine.Analysis.Presets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.Tests.Analysis {
	public class MonteCarloPresetTests {
		StockInvestment TestInvestment { get; init; }

		public MonteCarloPresetTests() {
			TestInvestment = new StockInvestment("") {
				StockPrice = 100,
				AnalysisOptionsOverrides = {
					["analysisLength"] = "50",
					["analysisPreset"] = ""
				}
			};

		}

		public static readonly IEnumerable<object[]> PresetResolutionData = new List<object[]>() {
			new object[]{"LargeCapGrowth", MonteCarloPresets.LargeCapGrowth},
			new object[] {"LargeCapValue", MonteCarloPresets.LargeCapValue },
			new object[] {"MidCapGrowth", MonteCarloPresets.MidCapGrowth },
			new object[] {"MidCapValue", MonteCarloPresets.MidCapValue},
			new object[] {"SmallCapGrowth", MonteCarloPresets.SmallCapGrowth},
			new object[] {"SmallCapValue", MonteCarloPresets.SmallCapValue},
			new object[] {"DefaultStockAnalysis", MonteCarloPresets.DefaultStockAnalysis},
			new object[]{"InternationalStock", MonteCarloPresets.InternationalStock},
		};
		public static readonly IEnumerable<object[]> PresetList = PresetResolutionData.Select(x => new object[] { x.First() });

		[Theory, MemberData(nameof(PresetResolutionData))]
		public void ResolveMonteCarloPresets_KnownPreset_ReturnsCorrectPreset(string preset, OptionsDict expectedSubset) {
			TestInvestment.AnalysisOptionsOverrides["analysisPreset"] = preset;

			var actual = MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict());

			actual.Should().IntersectWith(expectedSubset);
			actual.Should().HaveCount(expectedSubset.Count + 2);
			actual.Should().ContainKey("basePrice");
			actual["basePrice"].Should().Be(TestInvestment.StockPrice.ToString());
			actual.Should().ContainKey("analysisLength");
			actual["analysisLength"].Should().Be(TestInvestment.AnalysisOptionsOverrides["analysisLength"]);
		}

		[Theory, MemberData(nameof(PresetList))]
		public void ResolveMonteCarloPresets_NoDefinedAnalysisLength_UsesDefaultLength(string preset) {
			TestInvestment.AnalysisOptionsOverrides["analysisPreset"] = preset;
			TestInvestment.AnalysisOptionsOverrides.Remove("analysisLength");

			var actual = MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict());

			actual.Should().ContainKey("analysisLength");
			actual["analysisLength"].Should().Be("60");
		}

		[Theory, MemberData(nameof(PresetResolutionData))]
		public void ResolveMonteCarloPresets_KnownPresetDefinedInOverrideParam_ReturnsCorrectPresetUsingOverrideDef(string preset, OptionsDict expectedSubset) {
			var overrideParams = new OptionsDict() {
				["analysisPreset"] = preset
			};

			var actual = MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, overrideParams);

			actual.Should().IntersectWith(expectedSubset);
			actual.Should().HaveCount(expectedSubset.Count + 2);
			actual.Should().ContainKey("basePrice");
			actual["basePrice"].Should().Be(TestInvestment.StockPrice.ToString());
			actual.Should().ContainKey("analysisLength");
			actual["analysisLength"].Should().Be(TestInvestment.AnalysisOptionsOverrides["analysisLength"]);

		}

		[Fact]
		public void ResolveMonteCarloPresets_CustomPreset_ReturnsCustomPresetDefinedInInvestment() {
			TestInvestment.AnalysisOptionsOverrides["analysisPreset"] = "Custom";
			TestInvestment.AnalysisOptionsOverrides["randomVariableType"] = "Test1";
			TestInvestment.AnalysisOptionsOverrides["randomVariableMu"] = "Mu";
			TestInvestment.AnalysisOptionsOverrides["randomVariableSigma"] = "Sigma";
			TestInvestment.AnalysisOptionsOverrides["randomVariableScaleFactor"] = "ScaleFactor";
			TestInvestment.AnalysisOptionsOverrides["simCount"] = "100000";

			var actual = MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict());

			actual.Should().ContainKey("randomVariableType");
			actual["randomVariableType"].Should().Be("Test1");
			actual.Should().ContainKey("randomVariableMu");
			actual["randomVariableMu"].Should().Be("Mu");
			actual.Should().ContainKey("randomVariableSigma");
			actual["randomVariableSigma"].Should().Be("Sigma");
			actual.Should().ContainKey("randomVariableScaleFactor");
			actual["randomVariableScaleFactor"].Should().Be("ScaleFactor");
			actual.Should().ContainKey("simCount");
			actual["simCount"].Should().Be("100000");
		}

		[Fact]
		public void ResolveMonteCarloPresets_CustomPresetDefinedInOverrideParam_ReturnsCustomUsingOverrideParams() {
			var overrideParams = new OptionsDict() {
				["analysisPreset"] = "Custom",
				["randomVariableType"] = "Test1",
				["randomVariableMu"] = "Mu",
				["randomVariableSigma"] = "Sigma",
				["randomVariableScaleFactor"] = "ScaleFactor",
				["simCount"] = "100000"
			};

			var actual = MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, overrideParams);

			actual.Should().ContainKey("randomVariableType");
			actual["randomVariableType"].Should().Be("Test1");
			actual.Should().ContainKey("randomVariableMu");
			actual["randomVariableMu"].Should().Be("Mu");
			actual.Should().ContainKey("randomVariableSigma");
			actual["randomVariableSigma"].Should().Be("Sigma");
			actual.Should().ContainKey("randomVariableScaleFactor");
			actual["randomVariableScaleFactor"].Should().Be("ScaleFactor");
			actual.Should().ContainKey("simCount");
			actual["simCount"].Should().Be("100000");
		}

		[Fact]
		public void ResolveMonteCarloPresets_UnknownPreset_ThrowsException() {
			Action act = () => { MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict()); };
			act.Should().Throw<KeyNotFoundException>();
		}

		[Fact]
		public void ResolveMonteCarloPresets_UnknownPresetDefinedInOverrideParam_ThrowsException() {
			Action act = () => { MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict() { ["analysisPreset"] = "Unknown" }); };
			act.Should().Throw<KeyNotFoundException>();
		}

	}
}
