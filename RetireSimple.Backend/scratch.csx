#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\RetireSimple.Backend.dll"

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;

var investment = new StockInvestment("testAnalysis");
var investment2 = new StockInvestment("testAnalysis2");
investment.InvokeAnalysis();
investment2.InvokeAnalysis();