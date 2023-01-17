using RetireSimple.Backend.Services;

namespace RetireSimple.Backend.Api {

	/// <summary>
	/// Defines common operations that may be perfomed on an investment vehicle.
	/// Requires passing an <see cref="InvestmentDBContext"/> instance on construction to use
	/// </summary>
	public class InvestmentVehicle {
		private readonly InvestmentDBContext _context;

		public InvestmentVehicle(InvestmentDBContext context) {
			_context = context;
		}

		public void Add() {
		
		}

	}
}
