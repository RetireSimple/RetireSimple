#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\RetireSimple.Backend.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.Extensions.DependencyInjection.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.Extensions.Caching.Memory.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.Extensions.DependencyModel.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.Data.Sqlite.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.EntityFrameworkCore.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.EntityFrameworkCore.Abstractions.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.EntityFrameworkCore.Relational.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\MySqlConnector.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.dependencyinjection.abstractions\6.0.0\lib\net6.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.logging.abstractions\6.0.0\lib\net6.0\Microsoft.Extensions.Logging.Abstractions.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.caching.abstractions\6.0.0\lib\netstandard2.0\Microsoft.Extensions.Caching.Abstractions.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.logging\6.0.0\lib\netstandard2.1\Microsoft.Extensions.Logging.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.options\6.0.0\lib\netstandard2.1\Microsoft.Extensions.Options.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.configuration.abstractions\6.0.0\lib\netstandard2.0\Microsoft.Extensions.Configuration.Abstractions.dll"
#r "C:\Users\awest\.nuget\packages\microsoft.extensions.primitives\6.0.0\lib\net6.0\Microsoft.Extensions.Primitives.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\SQLitePCLRaw.core.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\SQLitePCLRaw.provider.e_sqlite3.dll"
#r "D:\Onedrive\SeniorProject\RetireSimple\RetireSimple.Backend\bin\Debug\net6.0\SQLitePCLRaw.batteries_v2.dll"

using RetireSimple.Backend.DomainModel.Data;
using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.Services;

var context = new InvestmentDBContext();

var investment = new StockInvestment("testAnalysis");
var investment2 = new StockInvestment("testAnalysis2");

context.Investments.Add(investment);
context.Investments.Add(investment2);

context.SaveChanges();

context.InvestmentModels.Add(investment.InvokeAnalysis());
context.InvestmentModels.Add(investment2.InvokeAnalysis());
context.SaveChanges();