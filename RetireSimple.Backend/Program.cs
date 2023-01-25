using Microsoft.EntityFrameworkCore;
using RetireSimple.Engine.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration["Provider"] ??= "sqlite";

builder.Services.AddControllers()
	.AddJsonOptions(options => {
		options.JsonSerializerOptions.AllowTrailingCommas = true;
		options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
		options.JsonSerializerOptions.IncludeFields = true;
	});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InvestmentDBContext>(options => _ =
	builder.Configuration["Provider"] switch {
		"sqlite" =>
			options.UseSqlite("Data Source=InvestmentDB.db"),
		_ => throw new ArgumentException("Invalid provider")
	});


var app = builder.Build();

//Only Apply Migrations for Sqlite
if(app.Configuration["Provider"] == "sqlite") {
	using(var scope = app.Services.CreateScope()) {

		var context = scope.ServiceProvider.GetRequiredService<InvestmentDBContext>();
		context.Database.Migrate();
		context.Database.EnsureCreated();
	}
}

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

if(app.Environment.IsProduction()) {
	app.UseStaticFiles();
}

app.MapControllers();

//invoke browser to app window
if(app.Environment.IsProduction()) {
	var url = "http://localhost:5000/index.html";
	try {
		Process.Start(url);
	} catch {
		// hack because of this: https://github.com/dotnet/corefx/issues/10361
		if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
			url = url.Replace("&", "^&");
			Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
		} else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
			Process.Start("xdg-open", url);
		} else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
			Process.Start("open", url);
		} else {
			throw;
		}
	}
}

app.Run();


