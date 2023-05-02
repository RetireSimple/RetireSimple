using RetireSimple.Engine.Analysis.Presets;

namespace RetireSimple.Tests.Analysis {
	public class RegressionPresetTests {
		StockInvestment TestInvestment { get; init; }

		public RegressionPresetTests() {
			TestInvestment = new StockInvestment("") {
				StockPrice = 100,
				AnalysisOptionsOverrides = {
					["analysisLength"] = "50",
					["analysisPreset"] = ""
				},
				StockQuantity = 5,
			};

		}

		public static readonly IEnumerable<object[]> PresetResolutionData = new List<object[]>() {
			new object[] { "LargeCap", RegressionPresets.LargeCap},
			new object[] { "MidCap", RegressionPresets.MidCap },
			new object[] { "SmallCap", RegressionPresets.SmallCap },
			new object[] { "HighVolatility", RegressionPresets.HighVolatility },
		};
		public static readonly IEnumerable<object[]> PresetList = PresetResolutionData.Select(x => new object[] { x.First() });

		[Theory, MemberData(nameof(PresetResolutionData))]
		public void ResolveRegressionPresets_KnownPreset_ReturnsCorrectPreset(string preset, OptionsDict expectedSubset) {
			TestInvestment.AnalysisOptionsOverrides["analysisPreset"] = preset;

			var actual = RegressionPresets.ResolveRegressionPreset(TestInvestment, new OptionsDict());

			actual.Should().IntersectWith(expectedSubset);
			actual.Should().HaveCount(expectedSubset.Count + 3);
			actual.Should().ContainKey("basePrice");
			actual["basePrice"].Should().Be(TestInvestment.StockPrice.ToString());
			actual.Should().ContainKey("analysisLength");
			actual["analysisLength"].Should().Be(TestInvestment.AnalysisOptionsOverrides["analysisLength"]);
			actual["stockQuantity"].Should().Be(TestInvestment.StockQuantity.ToString());
		}

		[Theory, MemberData(nameof(PresetList))]
		public void ResolveRegressionPresets_NoDefinedAnalysisLength_UsesDefaultLength(string preset) {
			TestInvestment.AnalysisOptionsOverrides["analysisPreset"] = preset;
			TestInvestment.AnalysisOptionsOverrides.Remove("analysisLength");

			var actual = RegressionPresets.ResolveRegressionPreset(TestInvestment, new OptionsDict());

			actual.Should().ContainKey("analysisLength");
			actual["analysisLength"].Should().Be("60");
		}

		[Theory, MemberData(nameof(PresetResolutionData))]
		public void ResolveRegressionPresets_KnownPresetDefinedInOverrideParam_ReturnsCorrectPresetUsingOverrideDef(string preset, OptionsDict expectedSubset) {
			var overrideParams = new OptionsDict() {
				["analysisPreset"] = preset
			};

			var actual = RegressionPresets.ResolveRegressionPreset(TestInvestment, overrideParams);

			actual.Should().IntersectWith(expectedSubset);
			actual.Should().HaveCount(expectedSubset.Count + 3);
			actual.Should().ContainKey("basePrice");
			actual["basePrice"].Should().Be(TestInvestment.StockPrice.ToString());
			actual.Should().ContainKey("analysisLength");
			actual["analysisLength"].Should().Be(TestInvestment.AnalysisOptionsOverrides["analysisLength"]);
			actual["stockQuantity"].Should().Be(TestInvestment.StockQuantity.ToString());
		}

		[Fact]
		public void ResolveRegressionPresets_CustomPreset_ReturnsCustomPresetDefinedInInvestment() {
			TestInvestment.AnalysisOptionsOverrides["analysisPreset"] = "Custom";
			TestInvestment.AnalysisOptionsOverrides["percentGrowth"] = "0.0102";
			TestInvestment.AnalysisOptionsOverrides["uncertainty"] = "0.48";

			var actual = RegressionPresets.ResolveRegressionPreset(TestInvestment, new OptionsDict());

			actual.Should().ContainKey("percentGrowth");
			actual["percentGrowth"].Should().Be("0.0102");
			actual.Should().ContainKey("uncertainty");
			actual["uncertainty"].Should().Be("0.48");
		}

		[Fact]
		public void ResolveRegressionPresets_CustomPresetDefinedInOverrideParam_ReturnsCustomUsingOverrideParams() {
			var overrideParams = new OptionsDict() {
				["analysisPreset"] = "Custom",
				["percentGrowth"] = "0.01",
				["uncertainty"] = "0.99",
			};

			var actual = RegressionPresets.ResolveRegressionPreset(TestInvestment, overrideParams);

			actual.Should().ContainKey("percentGrowth");
			actual["percentGrowth"].Should().Be("0.01");
			actual.Should().ContainKey("uncertainty");
			actual["uncertainty"].Should().Be("0.99");

		}

		[Fact]
		public void ResolveRegressionPresets_UnknownPreset_ThrowsException() {
			Action act = () => { MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict()); };
			act.Should().Throw<KeyNotFoundException>();
		}

		[Fact]
		public void ResolveRegressionPresets_UnknownPresetDefinedInOverrideParam_ThrowsException() {
			Action act = () => { MonteCarloPresets.ResolveMonteCarloPreset(TestInvestment, new OptionsDict() { ["analysisPreset"] = "Unknown" }); };
			act.Should().Throw<KeyNotFoundException>();
		}

	}
}

