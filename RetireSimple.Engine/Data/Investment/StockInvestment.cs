using RetireSimple.Engine.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment
{

    public class StockInvestment : InvestmentBase
    {

        /// <summary>
        /// The price of the Stock
        /// </summary>
        [JsonIgnore, NotMapped]
        public decimal StockPrice
        {
            get => decimal.Parse(InvestmentData["stockPrice"]);
            set => InvestmentData["stockPrice"] = value.ToString();
        }

        /// <summary>
        /// The Ticker Symbol of the Stock. Currently only used for identification purposes
        /// </summary>
        [JsonIgnore, NotMapped]
        public string StockTicker
        {
            get => InvestmentData["stockTicker"];
            set => InvestmentData["stockTicker"] = value;
        }

        /// <summary>
        /// The number of shares of the stock held
        /// </summary>
        [JsonIgnore, NotMapped]
        public decimal StockQuantity
        {
            get => decimal.Parse(InvestmentData["stockQuantity"]);
            set => InvestmentData["stockQuantity"] = value.ToString();
        }

        /// <summary>
        /// The date the stock was purchased. Stores as a Date-Time, but only the date is used. May change before formal release
        /// </summary>
        [JsonIgnore, NotMapped]
        public DateTime StockPurchaseDate
        {
            get => DateTime.Parse(InvestmentData["stockPurchaseDate"]);
            set => InvestmentData["stockPurchaseDate"] = value.ToString();
        }

        /// <summary>
        /// Percentage dividend that the stock pays. This is a decimal value between 0 and 1.
        /// </summary>
        [JsonIgnore, NotMapped]
        public decimal StockDividendPercent
        {
            get => decimal.Parse(InvestmentData["stockDividendPercent"]);
            set => InvestmentData["stockDividendPercent"] = value.ToString();
        }

        //TODO validation maybe?
        /// <summary>
        /// The interval upon which dividends are paid. Valid values are "Month", "Quarter", "Annual"
        /// </summary>
        [JsonIgnore, NotMapped]
        public string StockDividendDistributionInterval
        {
            get => InvestmentData["stockDividendDistributionInterval"];
            set => InvestmentData["stockDividendDistributionInterval"] = value;
        }

        //TODO validation maybe?
        /// <summary>
        /// Method of distribution of the dividends. Valid Values are "Stock", "Cash", "DRIP"
        /// </summary>
        [JsonIgnore, NotMapped]
        public string StockDividendDistributionMethod
        {
            get => InvestmentData["stockDividendDistributionMethod"];
            set => InvestmentData["stockDividendDistributionMethod"] = value;
        }

        /// <summary>
        /// A date to project the Dividend Payment months (Simplified to reduce information needed)
        /// </summary>
        [JsonIgnore, NotMapped]
        public DateTime StockDividendFirstPaymentDate
        {
            get => DateTime.Parse(InvestmentData["stockDividendFirstPaymentDate"]);
            set => InvestmentData["stockDividendFirstPaymentDate"] = value.ToString();
        }

        [JsonIgnore, NotMapped]
        public AnalysisDelegate<StockInvestment>? AnalysisMethod;

        //Constructor used by EF
        public StockInvestment(string analysisType) : base()
        {
            InvestmentType = "StockInvestment";
            ResolveAnalysisDelegate(analysisType);

        }

        public override void ResolveAnalysisDelegate(string analysisType)
        {
            switch (analysisType)
            {
                case "testAnalysis":
                    AnalysisMethod = StockAS.testAnalysis;
                    break;
                case "MonteCarlo_NormalDist":
                    AnalysisMethod = StockAS.MonteCarlo_NormalDist;
                    break;
                case "MonteCarlo_LogNormalDist":
                    AnalysisMethod = StockAS.MonteCarlo_LogNormalDist;
                    break;
                default:
                    AnalysisMethod = null;
                    break;

            }

            //Overwrite The current Analysis Delegate Type 
            AnalysisType = analysisType;
        }

        public override InvestmentModel InvokeAnalysis(OptionsDict options) =>
            AnalysisMethod is not null
            ? AnalysisMethod(this, options)
            : throw new InvalidOperationException("The specified investment has no specified analysis");
    }
}