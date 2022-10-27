//This is in an orphan project separate from the solution that references the backend project
//Will be removed in the future, useful now for trying to check the EF manually without spawning a webserver
//only executable through dotnet cli by intention

using Microsoft.EntityFrameworkCore;

using RetireSimple.Backend.Services;

var context = new InvestmentDBContext();
context.Database.Migrate();
context.Database.EnsureCreated();