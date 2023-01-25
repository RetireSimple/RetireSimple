using RetireSimple.Engine.Analysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RetireSimple.Engine.Data.Investment
{
    public class PensionInvestment : InvestmentBase
    {

        [JsonIgnore, NotMapped]
        public DateOnly PensionStartDate
        {
            get => DateOnly.Parse(InvestmentData["PensionStartDate"]);
            set => InvestmentData["PensionStartDate"] = value.ToString();
        }

        [JsonIgnore, NotMapped]
        public decimal PensionInitialMonthlyPayment
        {
            get => decimal.Parse(InvestmentData["PensionInitialMonthlyPayment"]);
            set => InvestmentData["PensionInitialMonthlyPayment"] = value.ToString();
        }

        [JsonIgnore, NotMapped]
        public decimal PensionYearlyIncrease
        {
            get => decimal.Parse(InvestmentData["PensionYearlyIncrease"]);
            set => InvestmentData["PensionYearlyIncrease"] = value.ToString();
        }

        public AnalysisDelegate<PensionInvestment>? AnalysisMethod;

        //Constructor used by EF
        public PensionInvestment(string analysisType) : base()
        {
            InvestmentType = "PensionInvestment";
            ResolveAnalysisDelegate(analysisType);
        }
        public override void ResolveAnalysisDelegate(string analysisType)
        {
            switch (analysisType)
            {
                case "DefaultCashAnalysis":
                    AnalysisMethod = PensionAS.DefaultPensionAnalysis;
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