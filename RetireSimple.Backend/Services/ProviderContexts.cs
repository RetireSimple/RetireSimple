using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace RetireSimple.Backend.Services {
	public class SqliteInvestmentContext : InvestmentDBContext {

		public SqliteInvestmentContext(DbContextOptions<SqliteInvestmentContext> options) : base(options) {
		}

		public static SqliteInvestmentContext MakeTestContext(string connectionString = "Data Source=InvestmentDB.db") {
			return new SqliteInvestmentContext(new DbContextOptionsBuilder<SqliteInvestmentContext>().UseSqlite(connectionString).Options);
		}
	}

	public class MariaDBInvestmentContext : InvestmentDBContext {
		private readonly string _connectionString;

		public MariaDBInvestmentContext(DbContextOptions options) : base(options) {
		}

		//public MariaDBInvestmentContext(string connectionString) {
		//	_connectionString = connectionString;
		//}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseMySql(ServerVersion.AutoDetect(_connectionString),
									options => options.EnableRetryOnFailure());
		}

	}
}