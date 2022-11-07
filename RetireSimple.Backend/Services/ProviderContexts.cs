using Microsoft.EntityFrameworkCore;

namespace RetireSimple.Backend.Services {
	public class SqliteInvestmentContext : InvestmentDBContext {
		private readonly string _connectionString;

		public SqliteInvestmentContext(string connectionString = "Data Source=InvestmentDB.db") {
			this._connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
			optionsBuilder.UseSqlite(_connectionString);
	}

	public class MariaDBInvestmentContext : InvestmentDBContext {
		private readonly string _connectionString;

		public MariaDBInvestmentContext(string connectionString) {
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseMySql(ServerVersion.AutoDetect(_connectionString),
									options => options.EnableRetryOnFailure());
		}

	}
}