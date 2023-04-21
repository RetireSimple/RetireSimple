using Microsoft.Extensions.Options;

using System.Collections;

namespace RetireSimple.Tests.Analysis {
	public class PensionSimTests {
		PensionInvestment Investment { get; init; }
		OptionsDict options { get; init; }

		public PensionSimTests() {
			Investment = new PensionInvestment("PensionSimulation") {
				PensionStartDate = DateOnly.FromDateTime(DateTime.Now),
				PensionInitialMonthlyPayment = 1000,
				PensionYearlyIncrease = 0.01M,
				AnalysisOptionsOverrides = new() {
					["analysisLength"] = "10",
					["expectedTaxRate"] = "0.0",
				}
			};
			options = new OptionsDict();
		}

		[Fact]
		public void PensionSim_PensionStartsBeyondAnalysisPeriod_ReturnsAllZeros() {
			Investment.PensionStartDate = DateOnly.FromDateTime(DateTime.Now.AddYears(10));

			var result = PensionAS.PensionSimulation(Investment, options);
			result.MinModelData.Should().AllBeEquivalentTo(0);
			result.MaxModelData.Should().AllBeEquivalentTo(0);
			result.AvgModelData.Should().AllBeEquivalentTo(0);
		}

		public static readonly List<decimal> ExpectedPensionValuesCase1 = new(){
			1000,2000,3000, 4000,5000,6000,7000,8000,9000,10000, 11000, 12000, 13000, 14000, 15000, 16000, 17000, 18000, 19000, 20000
		};

		public static readonly IEnumerable<object[]> StartOffset = Enumerable.Range(0, 10).Select(x => new object[] { x });
		[Theory, MemberData(nameof(StartOffset))]
		public void PensionSim_PensionStartsDuringAnalysisPeriod_OffsetsByNecessaryZeroes(int startOffset) {
			Investment.PensionStartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(startOffset));

			var result = PensionAS.PensionSimulation(Investment, options);
			result.MinModelData.Should().StartWith(Enumerable.Repeat(0M, startOffset));
			result.MaxModelData.Should().StartWith(Enumerable.Repeat(0M, startOffset));
			result.AvgModelData.Should().StartWith(Enumerable.Repeat(0M, startOffset));

			result.MinModelData.Should().EndWith(ExpectedPensionValuesCase1.Take(10 - startOffset));
			result.MaxModelData.Should().EndWith(ExpectedPensionValuesCase1.Take(10 - startOffset));
			result.AvgModelData.Should().EndWith(ExpectedPensionValuesCase1.Take(10 - startOffset));
		}

		[Theory, MemberData(nameof(StartOffset))]
		public void PensionSim_PensionAlreadyStarted_SimulatesCorrectly(int startOffset) {
			Investment.PensionYearlyIncrease = 0;
			Investment.PensionStartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-startOffset));

			var result = PensionAS.PensionSimulation(Investment, options);
			result.MinModelData.Should().StartWith(ExpectedPensionValuesCase1.Skip(startOffset).Take(10));
			result.MaxModelData.Should().StartWith(ExpectedPensionValuesCase1.Skip(startOffset).Take(10));
			result.AvgModelData.Should().StartWith(ExpectedPensionValuesCase1.Skip(startOffset).Take(10));
		}

		public static readonly List<decimal> ExpectedPensionValuesWithYearlyIncrease = new() {
			1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000,
			13100, 14200, 15300, 16400, 17500, 18600, 19700, 20800, 21900, 23000, 24100,
			25310, 26520, 27730, 28940, 30150, 31360, 32570, 33780, 34990, 36200, 37410,
		};
		[Fact]
		public void PensionSim_PensionHasNonZeroYearlyIncrease_SimulatesCorrectly() {
			Investment.PensionYearlyIncrease = 0.1M;
			options["analysisLength"] = "36";

			var result = PensionAS.PensionSimulation(Investment, options);
			result.MinModelData.Should().StartWith(ExpectedPensionValuesWithYearlyIncrease.Take(10));
			result.MaxModelData.Should().StartWith(ExpectedPensionValuesWithYearlyIncrease.Take(10));
			result.AvgModelData.Should().StartWith(ExpectedPensionValuesWithYearlyIncrease.Take(10));
		}

		[Fact]
		public void PensionSim_UsesOverrideValuesAsExpected() {
			//Change the base values
		}

		public static readonly List<decimal> ExpectedPensionValuesUsingTaxRate = new() {
			900, 1800, 2700, 3600, 4500, 5400, 6300, 7200, 8100, 9000, 9900, 10800,
		};
		[Fact]
		public void PensionSim_UsesTaxRateAsExpected() {
			options["expectedTaxRate"] = "0.1";

			var result = PensionAS.PensionSimulation(Investment, options);
			result.MinModelData.Should().StartWith(ExpectedPensionValuesUsingTaxRate.Take(10));
			result.MaxModelData.Should().StartWith(ExpectedPensionValuesUsingTaxRate.Take(10));
			result.AvgModelData.Should().StartWith(ExpectedPensionValuesUsingTaxRate.Take(10));
		}

	}

}