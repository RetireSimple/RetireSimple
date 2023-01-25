using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Investment;
using RetireSimple.Engine.Data.InvestmentVehicle;
using RetireSimple.Engine.DomainModel.Data.InvestmentVehicle;

namespace RetireSimple.Engine.Api
{

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
		/// Gest a specific investment vehicle by its ID. This is useful 
		/// for retreiving a specific vehicle after it goes through multiple updates
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">If no investment vehicle with the specified ID exists</exception>
		public InvestmentVehicleBase GetInvestmentVehicle(int vehicleId) {
			return _context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)
				? _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId)
				: throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
		}

		/// <summary>
		/// Creates a new InvestmentVehicle. During the creation process, 
		/// a <see cref="CashInvestment"/> is created and bound to the internal 
		/// relationship fields. 
		/// </summary>
		public int Add(string type, string name) {
			InvestmentVehicleBase vehicle = type switch {
				"401k" => new Vehicle401k(),
				"403b" => new Vehicle403b(),
				"457" => new Vehicle457(),
				"IRA" => new VehicleIRA(),
				"RothIRA" => new VehicleRothIRA(),
				_ => throw new ArgumentException("Unknown Investment Vehicle type")
			};

			vehicle.InvestmentVehicleName = name;
			_context.Portfolio.First().InvestmentVehicles.Add(vehicle);
			_context.SaveChanges();

			return vehicle.InvestmentVehicleId;
		}

		/// <summary>
		/// Adds an investment to the vehicle. The investment must exist in the database 
		/// before this operation, otherwise an <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <param name="vehicleId">ID of the InvestmentVehicleBase object</param>
		/// <param name="investmentId">ID of the InvestmentBase object</param>
		public void AddInvestmentToVehicle(int vehicleId, int investmentId) {
			//TODO Inline validation (eventually)
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
			}
			if (!_context.Investment.Any(i => i.InvestmentId == investmentId)) {
				throw new ArgumentException($"Investment with ID {investmentId} does not exist");
			}

			var vehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			var investment = _context.Investment.First(i => i.InvestmentId == investmentId);
			vehicle.Investments.Add(investment);
			_context.SaveChanges();
		}

		/// <summary>
		/// Removes an investment from an investment vehicle, then deletes the investment.
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <param name="investmentId"></param>
		/// <exception cref="ArgumentException">If the investment or investment vehicle does not exist, or if the investment is not found in the investment vehicle</exception>
		public void RemoveInvestmentFromVehicle(int vehicleId, int investmentId) {
			//TODO Inline validation (eventually)
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
			}
			if (!_context.Investment.Any(i => i.InvestmentId == investmentId)) {
				throw new ArgumentException($"Investment with ID {investmentId} does not exist");
			}
			if (!_context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId)
				.Investments.Any(i => i.InvestmentId == investmentId)) {
				throw new ArgumentException($"Vehicle with ID {vehicleId} does contain an investment with ID {investmentId}");
			}

			var removeInvestment = _context.Investment.First(i => i.InvestmentId == investmentId);
			_context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId)
				.Investments.Remove(removeInvestment);
			_context.Investment.Remove(removeInvestment);
			_context.SaveChanges();
		}

		public void Remove(int vehicleId) {
			//TODO Inline validation (eventually)
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
			}

			var removeVehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			_context.InvestmentVehicle.Remove(removeVehicle);
			_context.SaveChanges();
		}

		/// <summary>
		/// Updates the <see cref="InvestmentVehicleBase.AnalysisOptionsOverrides"/> with the 
		/// specified analysis option and option value. No actual validation of the value with 
		/// respect to the analysis option is performed in this method. If <paramref name="value"/>
		/// is null and the option exists, it gets removed from the analysis overrides for the
		/// vehicle.
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <param name="option"></param>
		/// <param name="value"></param>
		/// <exception cref="ArgumentException">If the specified option does not exist yet in
		/// the overrides and <paramref name="value"/> is null</exception>
		public void UpdateAnalysisOverrides(int vehicleId, string option, string? value) {
			//TODO Inline validation (eventually)
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
			}

			var vehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);

			if (!vehicle.AnalysisOptionsOverrides.ContainsKey(option) && value is null) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not have the analysis option {option} set, so it cannot be removed!");
			}

			if (value is null) {
				vehicle.AnalysisOptionsOverrides.Remove(option);
			}
			else {

				vehicle.AnalysisOptionsOverrides[option] = value;
			}

			_context.SaveChanges();
		}


	}
}
