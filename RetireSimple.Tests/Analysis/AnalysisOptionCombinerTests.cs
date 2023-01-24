using RetireSimple.Backend.DomainModel.Analysis;
using RetireSimple.Backend.DomainModel.Data.Investment;

using System;
namespace RetireSimple.Tests
{
    public class AnalysisOptionCombinerTests
    {
        OptionsDict dict, finalDict, investmentDict;
        InvestmentBase investment;

        public AnalysisOptionCombinerTests()
        {
            dict = new OptionsDict();
            dict.Add("test", "result");

            finalDict = new OptionsDict();

            finalDict.Add("investment", "result");
            finalDict.Add("test", "result");
            finalDict.Add("AnalysisLength", "60");

            investmentDict = new OptionsDict();
            investmentDict.Add("investment", "result");

            investment = new StockInvestment("test");
            investment.AnalysisOptionsOverrides = investmentDict;


        }

        [Fact]
        public void TestStockAS() {
            var newDict = StockAS.stockAnalysisOption(investment, dict);
            finalDict.Add("StockAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestSSAS() {
            var newDict = SSAS.SSAnalysisOption(investment, dict);
            finalDict.Add("SSAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestSocialSecurityAS() {
            var newDict = SocialSecurityAS.SocialSecurityAnalysisOption(investment, dict);
            finalDict.Add("SocialSecurityAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestPensionAS() {
            var newDict = PensionAS.PensionAnalysisOption(investment, dict);
            finalDict.Add("PensionAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestFixedAS() {
            var newDict = FixedAS.FixedAnalysisOption(investment, dict);
            finalDict.Add("FixedAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestCashAS() {
            var newDict = CashAS.CashAnalysisOption(investment, dict);
            finalDict.Add("CashAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestBondAS() {
            var newDict = BondAS.BondAnalysisOption(investment, dict);
            finalDict.Add("BondAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestAnnuityAS() {
            var newDict = AnnuityAS.AnnuityAnalysisOption(investment, dict);
            finalDict.Add("AnnuityAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }

        [Fact]
        public void TestMonteCarloAS() {
            var newDict = MonteCarlo.MonteCarloAnalysisOption(investment, dict);
            finalDict.Add("MonteCarloAnalysisExpectedGrowth", "0.1");
            Assert.Equal(newDict, finalDict);
        }
    }
}

