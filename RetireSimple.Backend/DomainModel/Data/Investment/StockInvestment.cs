using RetireSimple.Backend.DomainModel.Analysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetireSimple.Backend.DomainModel.Data.Investment {
	public abstract class StockInvestment : InvestmentBase {

		[NotMapped]
		public double StockPrice { get => double.Parse(this.InvestmentData["StockPrice"]); set => this.InvestmentData["StockPrice"] = value.ToString(); }
		[NotMapped]
		public string StockTicker { get => this.InvestmentData["StockTicker"]; set => this.InvestmentData["StockTicker"] = value; }
	}
}