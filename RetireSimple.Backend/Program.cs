using Microsoft.EntityFrameworkCore;

using RetireSimple.Engine.Data;
using RetireSimple.NewEngine.New_Engine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction()) {
	if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RetireSimple"))) {
		Console.WriteLine("Creating directory");
		Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RetireSimple"));
	}
	builder.Configuration["Data:EngineDbContext:ConnectionString"] = "Data Source="
		+ Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RetireSimple", "EngineDB.db");
} else {
	builder.Configuration["Data:EngineDbContext:ConnectionString"] = "Data Source=EngineDB.db";
}
Console.WriteLine(builder.Configuration["Data:EngineDbContext:ConnectionString"]);

builder.Services.AddControllers()
	.AddJsonOptions(options => {
		options.JsonSerializerOptions.AllowTrailingCommas = true;
		options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
		options.JsonSerializerOptions.IncludeFields = true;
	});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EngineDbContext>(options => {
	options.UseSqlite(builder.Configuration["Data:EngineDbContext:ConnectionString"]);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
	var context = scope.ServiceProvider.GetRequiredService<EngineDbContext>();
	context.Database.Migrate();
}

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

if (app.Environment.IsProduction()) {
	app.UseStaticFiles();
	app.UseDefaultFiles();
}

app.MapControllers();

app.Run();


