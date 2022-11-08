using Microsoft.EntityFrameworkCore;
using RetireSimple.Backend.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration["Provider"] ??= "sqlite";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Needed to ensure inherited context type overrides base type
//builder.Services.AddDbContext<InvestmentDBContext>(options =>
//	options.UseSqlite("Data Source=InvestmentDB.db"));

//switch(builder.Configuration["Provider"]) {
//	case "sqlite":
//		builder.Services.AddDbContext<SqliteInvestmentContext>(options => options.UseSqlite("Data Source=InvestmentDB.db"));
//		break;
//	case "mariadb":
//		builder.Services.AddDbContext<MariaDBInvestmentContext>();
//		break;
//	default:
//		throw new ArgumentException("Invalid provider");
//}

builder.Services.AddDbContext<InvestmentDBContext>(options => _ =
	builder.Configuration["Provider"] switch {
		"sqlite" =>
			options.UseSqlite("Data Source=InvestmentDB.db",
			x => x.MigrationsAssembly("RetireSimple.Migrations.Sqlite")
			),
		"mariadb" =>
			options.UseMySql(ServerVersion.AutoDetect(builder.Configuration["ConnectionString"]),
			x => x.MigrationsAssembly("RetireSimple.Migrations.MariaDB")),
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


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
