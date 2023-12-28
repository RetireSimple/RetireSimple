using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis {
	public class PensionAS {
		// A very simple Simulation of pensions/social security.
		[AnalysisModule(nameof(PensionInvestment))]
		public static InvestmentModel PensionSimulation(PensionInvestment investment, OptionsDict options) {
			var expectedTaxRate = decimal.Parse(options.GetValueOrDefault("expectedTaxRate")
												?? investment.AnalysisOptionsOverrides["expectedTaxRate"]);
			var pensionAmt = investment.PensionInitialMonthlyPayment;
			var increaseRate = 1 + investment.PensionYearlyIncrease;    //this is a percentage, not a fixed amount
			var pensionStart = investment.PensionStartDate;
			var analysisLength = int.Parse(options.GetValueOrDefault("analysisLength")
											?? investment.AnalysisOptionsOverrides["analysisLength"]);

			var monthsToSimulate = GetNumberOfSimIterations(analysisLength, pensionStart);
			var offset = GetStartingIterationOffset(pensionStart);
			var pensionValue = 0M;
			var pensionData = new List<decimal>();
			
			for (int i = 0; i < monthsToSimulate; i++) {
				pensionValue += pensionAmt * (1 - expectedTaxRate);
				if (i != 0 && i % 12 == 0) {
					pensionAmt *= increaseRate;
				}
				pensionData.Add(pensionValue);
			}


			//Offset with 0 as necessary
			if (offset > 0) {
				pensionData.InsertRange(0, Enumerable.Repeat(0M, offset));
			} else if (offset < 0) {
				pensionData.RemoveRange(0, -offset);
			}


			pensionData = pensionData.Take(analysisLength).ToList();

			return new InvestmentModel() {
				InvestmentId = investment.InvestmentId,
				MinModelData = pensionData,
				MaxModelData = pensionData,
				AvgModelData = pensionData,
			};
		}

		internal static int GetNumberOfSimIterations(int length, DateOnly pensionStart) {
			var simEnd = DateTime.Now.AddMonths(length);
			if (simEnd < new DateTime(pensionStart.Year, pensionStart.Month, 1)) {
				return 0;
			}
			return (simEnd.Year - pensionStart.Year) * 12 + (simEnd.Month - pensionStart.Month);
		}

		internal static int GetStartingIterationOffset(DateOnly pensionStart) {
			var offset = (pensionStart.Year - DateTime.Now.Year) * 12
						+ (pensionStart.Month - DateTime.Now.Month);

			return offset;
		}
	}
}