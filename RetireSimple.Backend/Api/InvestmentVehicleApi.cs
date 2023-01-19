using RetireSimple.Backend.DomainModel.Data.Investment;
using RetireSimple.Backend.DomainModel.Data.InvestmentVehicle;
using RetireSimple.Backend.Services;

namespace RetireSimple.Backend.Api {

	/// <summary>
	/// Defines common operations that may be perfomed on an investment vehicle.
	/// Requires passing an <see cref="InvestmentDBContext"/> instance on construction to use
	/// </summary>
	public class InvestmentVehicleApi {
		private readonly InvestmentDBContext _context;

		public InvestmentVehicleApi(InvestmentDBContext context) {
			_context = context;
		}

		/// <summary>
		/// Returns all investment vehicle objects currently tracked by EF. 
		/// </summary>
		/// <returns></returns>
		public List<InvestmentVehicleBase> GetInvestmentVehicles() {
			return _context.InvestmentVehicle.ToList();
		}

		/// <summary>
		/// Creates a new InvestmentVehicle. During the creation process, 
		/// a <see cref="CashInvestment"/> is created and bound to the internal 
		/// relationship fields. 
		/// </summary>
		public void Add(string type, string name) {
			InvestmentVehicleBase vehicle = type switch {
				"401k" => new Vehicle401k(),
				"403b" => new Vehicle403b(),
				"457" => new Vehicle457(),
				"IRA" => new VehicleIRA(),
				"RothIRA" => new VehicleRothIRA(),
				_ => throw new ArgumentException("Unknown Investment Vehicle type")
			};

			var cashInvestment = new CashInvestment("");
			cashInvestment.InvestmentName = $"{name} - Unallocated Capital";
			_context.Portfolio.First().Investments.Add(cashInvestment);
			
			vehicle.InvestmentVehicleName = name;
			vehicle.Investments.Add(cashInvestment);

			_context.Portfolio.First().InvestmentVehicles.Add(vehicle);
			_context.SaveChanges();
		}



		public void AddInvestmentToVehicle() {

		}

		public void RemoveInvestmentFromVehicle() {

		}

		public void Remove() {

		}

		public void UpdateAnalysisOverrides() {

		}


	}
}
