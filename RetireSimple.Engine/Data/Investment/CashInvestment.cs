using RetireSimple.Engine.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace RetireSimple.Engine.Data.Investment
{
    public class CashInvestment : InvestmentBase
    {

        [JsonIgnore, NotMapped]
        public string CashCurrency
        {
            get => InvestmentData["CashCurrency"];
            set => InvestmentData["CashCurrency"] = value;
        }

        [JsonIgnore, NotMapped]
        public decimal CashQuantity
        {
            get => decimal.Parse(InvestmentData["CashQuantity"]);
            set => InvestmentData["CashQuantity"] = value.ToString();
        }

        [JsonIgnore, NotMapped]
        public decimal CashCurrentValue
        {
            get => decimal.Parse(InvestmentData["CashValue"]);
            set => InvestmentData["CashValue"] = value.ToString();
        }

        [JsonIgnore, NotMapped]
        public AnalysisDelegate<CashInvestment>? AnalysisMethod;

        public CashInvestment(string analysisType) : base()
        {
            InvestmentType = "CashInvestment";
            ResolveAnalysisDelegate(analysisType);
        }

        public override void ResolveAnalysisDelegate(string analysisType)
        {
            switch (analysisType)
            {
                case "DefaultCashAnalysis":
                    AnalysisMethod = CashAS.DefaultCashAnalysis;
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
