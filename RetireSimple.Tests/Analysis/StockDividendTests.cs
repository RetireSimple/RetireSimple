namespace RetireSimple.Tests.Analysis {
    public class StockDividendTests {

        StockInvestment TestInvestment { get; set; }

        //First Payment List shared across tests. Typing required for using xUnit theory
        public static readonly IEnumerable<object[]> FirstPaymentList =
            Enumerable.Range(1, 12).Select(x => new object[] { new DateTime(2022, x, 1) });

        private readonly ITestOutputHelper output;


        public StockDividendTests(ITestOutputHelper output) {
            TestInvestment = new StockInvestment("");
            TestInvestment.StockPrice = 100;
            TestInvestment.StockQuantity = 100;
            TestInvestment.StockDividendPercent = 0.05m;
            this.output = output;
        }

        [Theory]
        [MemberData(nameof(FirstPaymentList))]
        public void TestStockDividendMonthlyStockDistribution(DateTime firstPayment) {
            TestInvestment.StockDividendDistributionInterval = "Month";
            TestInvestment.StockDividendFirstPaymentDate = firstPayment;

            var dividendList = StockAS.ProjectStockDividend(TestInvestment, StockAS.DefaultStockAnalysisOptions);

            var expectedQuantity = TestInvestment.StockQuantity;
            //Assert.Equal(expectedQuantity, dividendList[0]);
            for (int i = 0; i < dividendList.Count; i++) {
                expectedQuantity += (expectedQuantity * TestInvestment.StockDividendPercent);
                Assert.Equal(expectedQuantity, dividendList[i]);
            }
        }

        [Theory]
        [MemberData(nameof(FirstPaymentList))]
        public void TestStockDividendQuarterlyStockDistribution(DateTime firstPayment) {
            TestInvestment.StockDividendDistributionInterval = "Quarter";
            TestInvestment.StockDividendFirstPaymentDate = firstPayment;

            var dividendList = StockAS.ProjectStockDividend(TestInvestment, StockAS.DefaultStockAnalysisOptions);

            foreach (decimal d in dividendList) {
                output.WriteLine(d.ToString());
            }

            var expectedQuantity = TestInvestment.StockQuantity;
            var currentSimMonth = DateTime.Now.Month;
            for (int i = 0; i < dividendList.Count; i++) {
                if ((currentSimMonth - firstPayment.Month) % 3 == 0) {
                    expectedQuantity += (expectedQuantity * TestInvestment.StockDividendPercent);

                }

                Assert.Equal(expectedQuantity, dividendList[i]);
                if ((currentSimMonth++) % 12 == 0) {
                    currentSimMonth = 1;
                }
            }
        }

        [Theory]
        [MemberData(nameof(FirstPaymentList))]
        public void TestStockDividendAnnualStockDistribution(DateTime firstPayment) {
            TestInvestment.StockDividendDistributionInterval = "Annual";
            TestInvestment.StockDividendFirstPaymentDate = firstPayment;

            var dividendList = StockAS.ProjectStockDividend(TestInvestment, StockAS.DefaultStockAnalysisOptions);

            foreach (decimal d in dividendList) {
                output.WriteLine(d.ToString());
            }

            var expectedQuantity = TestInvestment.StockQuantity;
            var currentSimMonth = DateTime.Now.Month;
            for (int i = 0; i < dividendList.Count; i++) {
                if ((currentSimMonth - firstPayment.Month) % 12 == 0) {
                    expectedQuantity += (expectedQuantity * TestInvestment.StockDividendPercent);

                }

                Assert.Equal(expectedQuantity, dividendList[i]);
                if ((currentSimMonth++) % 12 == 0) {
                    currentSimMonth = 1;
                }
            }
        }
    }
}