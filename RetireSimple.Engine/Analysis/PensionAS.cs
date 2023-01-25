using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;

namespace RetireSimple.Engine.Analysis
{
    public class PensionAS
    {
        public static InvestmentModel DefaultPensionAnalysis(PensionInvestment investment, OptionsDict options)
        {
            throw new NotImplementedException();
        }

        public static readonly OptionsDict DefaultPensionAnalysisOptions = new()
        {
            ["AnalysisLength"] = "60",                          //Number of months to project
            ["PensionAnalysisExpectedGrowth"] = "0.1",            //Expected Percentage Growth of the stock

        };

        public static OptionsDict PensionAnalysisOption(InvestmentBase investment, OptionsDict dict)
        {
            var newDict = new OptionsDict(dict);
            var investmentOptions = investment.AnalysisOptionsOverrides;

            foreach (var k in investmentOptions.Keys)
            {
                newDict.TryAdd(k, investmentOptions[k]);
            }

            foreach (var k in DefaultPensionAnalysisOptions.Keys)
            {
                newDict.TryAdd(k, DefaultPensionAnalysisOptions[k]);
            }

            return newDict;
        }
    }
}