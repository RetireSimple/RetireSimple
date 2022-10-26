using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.Services;

var context = new InvestmentDBContext();

context.Database.EnsureCreated();
var investment = new StockInvestment("testAnalysis");
var investment2 = new StockInvestment("testAnalysis2");

context.Investments.Add(investment);
context.Investments.Add(investment2);

context.SaveChanges();

context.InvestmentModels.Add(investment.InvokeAnalysis());
context.InvestmentModels.Add(investment2.InvokeAnalysis());
context.SaveChanges();