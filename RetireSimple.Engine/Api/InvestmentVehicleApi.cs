using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.Data.Investment;
using RetireSimple.Engine.Data.InvestmentVehicle;

namespace RetireSimple.Engine.Api {

	/// <summary>
	/// Defines common operations that may be perfomed on an investment vehicle.
	/// Requires passing an <see cref="EngineDbContext"/> instance on construction to use
	/// </summary>
	public class InvestmentVehicleApi {
		private readonly EngineDbContext _context;

		public InvestmentVehicleApi(EngineDbContext context) {
			_context = context;
		}

		/// <summary>
		/// Returns all investment vehicle objects currently tracked by EF.
		/// </summary>
		/// <returns></returns>
		public List<InvestmentVehicle> GetInvestmentVehicles() {
			return _context.InvestmentVehicle.ToList();
		}

		/// <summary>
		/// Gest a specific investment vehicle by its ID. This is useful
		/// for retreiving a specific vehicle after it goes through multiple updates
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">If no investment vehicle with the specified ID exists</exception>
		public InvestmentVehicle GetInvestmentVehicle(int vehicleId) {
			return _context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)
				? _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId)
				: throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
		}

		/// <summary>
		///	Returns a dictionary showing which investments are contained in a vehicle. Does not
		/// include individual investments. Keys of the dictionary are the IDs of the vehicles,
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, List<Investment>> GetAllVehicleInvestments() {
			var dict = new Dictionary<int, List<Investment>>();

			if (_context.InvestmentVehicle.Any()) {
				foreach (var vehicle in _context.InvestmentVehicle) {
					dict.Add(vehicle.InvestmentVehicleId, vehicle.Investments.ToList());
				}
			}

			return dict;
		}

		/// <summary>
		/// Returns all the investments associated with a vehicle. Best used for getting the vehicle after
		/// updating it. Throws an <see cref="ArgumentException"/> if the vehicle does not exist.
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <returns></returns>
		public List<Investment> GetVehicleInvestments(int vehicleId) {
			return _context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)
				? _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId).Investments.ToList()
				: throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
		}

		/// <summary>
		/// Creates a new InvestmentVehicle. During the creation process,
		/// a <see cref="CashInvestment"/> is created and bound to the internal
		/// relationship fields.
		/// </summary>
		public int Add(string type, OptionsDict options) {
			var moduleType = typeof(InvestmentVehicle).Assembly.GetTypes().First(t => t.Name == $"{type}")
				?? throw new ArgumentException($"The investment vehicle {type} does not exist");
			var moduleCtor = moduleType.GetConstructor(Array.Empty<Type>())
				?? throw new InvalidOperationException($"The investment vehicle {type} does not have a parameterless constructor");
			var vehicle = (InvestmentVehicle)moduleCtor.Invoke(Array.Empty<object>())
				?? throw new InvalidOperationException($"Failed to create an instance of {type}");

			vehicle.InvestmentVehicleName = options["investmentVehicleName"];
			vehicle.CashHoldings = decimal.Parse(options.GetValueOrDefault("cashHoldings", "0"));
			_context.Portfolio.First().InvestmentVehicles.Add(vehicle);
			_context.SaveChanges();

			return vehicle.InvestmentVehicleId;
		}

		/// <summary>
		/// Adds an investment to the vehicle. The investment must exist in the database
		/// before this operation, otherwise an <see cref="ArgumentException"/> is thrown.
		/// </summary>
		/// <param name="vehicleId">ID of the InvestmentVehicle object</param>
		/// <param name="investmentId">ID of the Investment object</param>
		public void AddInvestmentToVehicle(int vehicleId, int investmentId) {
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

		/// <summary>
		/// Removes an Investment Vehicle, along with all associated investments, models, and cash holdings.
		/// </summary>
		/// <param name="vehicleId"></param>
		public void Remove(int vehicleId) {
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
			}

			var removeVehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			_context.InvestmentVehicle.Remove(removeVehicle);
			_context.SaveChanges();
		}

		/// <summary>
		///	Updates the name/cash holdings of the investment vehicle. Throws an <see cref="ArgumentException"/>
		/// if the vehicle does not exist. Ignores unkonwn fields present in <paramref name="options"/>.
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <param name="options">A dictionary of options to update. Currently only "investmentVehicleName" and "cashHoldings" are supported.</param>
		public void Update(int vehicleId, OptionsDict options) {
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException($"Investment Vehicle with ID {vehicleId} does not exist");
			}

			var vehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);

			//NOTE For now there are only two effective fields that can be updated, all other fields are
			//NOTE ignored. This is somewhat by intention.

			if (options.ContainsKey("investmentVehicleName")) {
				if (!string.IsNullOrWhiteSpace(options["investmentVehicleName"])) {
					vehicle.InvestmentVehicleName = options["investmentVehicleName"];
				} else {
					throw new ArgumentException("Name cannot be empty");
				}
			}

			if (options.ContainsKey("cashHoldings")) {
				if (!string.IsNullOrWhiteSpace(options["cashHoldings"])) {
					vehicle.CashHoldings = decimal.Parse(options["cashHoldings"]);
				} else {
					throw new ArgumentException("Cash Holdings cannot be empty");
				}
			}

			_context.SaveChanges();
		}

		/// <summary>
		/// Updates the <see cref="InvestmentVehicle.AnalysisOptionsOverrides"/> with the
		/// specified analysis option and option value. No actual validation of the value with
		/// respect to the analysis option is performed in this method. If the value for a key
		/// is empty, it is removed from the dictionary if it exists.
		/// </summary>
		/// <param name="vehicleId"></param>
		/// <param name="options">A dictionary of options to update. </param>
		/// <exception cref="ArgumentException">If the vehicle could not be found</exception>
		public void UpdateAnalysisOverrides(int vehicleId, OptionsDict options) {
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
				throw new ArgumentException("Investment Vehicle not found");
			}

			var investmentVehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			foreach (var (key, value) in options) {
				if (value == "") {
					investmentVehicle.AnalysisOptionsOverrides.Remove(key);
				} else {
					investmentVehicle.AnalysisOptionsOverrides[key] = value;
				}
			}
			investmentVehicle.LastUpdated = DateTime.Now;

			_context.SaveChanges();
		}


		/// <summary>
		/// Returns a model of the vehicle's value and based on the vehicle's defined analysis, set options, and (optionally)
		/// any parameters that should be used as an override. If an existing <see cref="VehicleModel"/>
		/// already exists and the investment's last change is before the time the model was generated, no analysis
		/// is executed and the existing model is returned. If <paramref name="options"/> is passed with a non-null
		/// value, the analysis is re-executed regardless if the model is up-to-date.
		///
		/// </summary>
		/// <param name="id"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		//TODO Need to figure out how to add testing (is based on InvestmentApi logic that works)
		public VehicleModel GetAnalysis(int id, OptionsDict? options = null) {
			if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == id)) {
				throw new ArgumentException("Investment Vehicle not found");
			}

			var vehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == id);
			if ((options is not null && options.Count > 0) ||
			vehicle.InvestmentVehicleModel is null ||
			vehicle.LastUpdated > vehicle.InvestmentVehicleModel.LastUpdated ||
			vehicle.Investments.Any(i => i.LastUpdated > vehicle.InvestmentVehicleModel.LastUpdated)) {
				var vehicleModel = vehicle.GenerateAnalysis(options ?? new OptionsDict());
				vehicle.InvestmentVehicleModel = vehicleModel;
				var updateTime = DateTime.Now;
				if (vehicle.InvestmentVehicleModel is not null) {
					vehicle.InvestmentVehicleModel.LastUpdated = updateTime;
					vehicle.InvestmentVehicleModel.MinModelData = vehicleModel.MinModelData;
					vehicle.InvestmentVehicleModel.AvgModelData = vehicleModel.AvgModelData;
					vehicle.InvestmentVehicleModel.MaxModelData = vehicleModel.MaxModelData;
					vehicle.InvestmentVehicleModel.TaxDeductedMinModelData = vehicleModel.TaxDeductedMinModelData;
					vehicle.InvestmentVehicleModel.TaxDeductedAvgModelData = vehicleModel.TaxDeductedAvgModelData;
					vehicle.InvestmentVehicleModel.TaxDeductedMaxModelData = vehicleModel.TaxDeductedMaxModelData;
				} else {
					vehicleModel.InvestmentVehicleId = vehicle.InvestmentVehicleId;
					vehicleModel.LastUpdated = updateTime;
					vehicle.InvestmentVehicleModel = vehicleModel;
				}

				vehicle.LastUpdated = updateTime;
				_context.SaveChanges();
			}

			//Zero Floor Data
			var tempModel = vehicle.InvestmentVehicleModel;
			tempModel.MinModelData = tempModel.MinModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.AvgModelData = tempModel.AvgModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.MaxModelData = tempModel.MaxModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.TaxDeductedMinModelData = tempModel.TaxDeductedMinModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.TaxDeductedAvgModelData = tempModel.TaxDeductedAvgModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.TaxDeductedMaxModelData = tempModel.TaxDeductedMaxModelData.Select(d => Math.Max(d, 0)).ToList();

			return tempModel;
		}

		//TODO: Implement Later when adding vehicle breakdowns
		public List<InvestmentModel> GetVehicleInvestmentModels(int vehicleId, OptionsDict? options = null) {
			// if (!_context.InvestmentVehicle.Any(i => i.InvestmentVehicleId == vehicleId)) {
			// 	throw new ArgumentException("Investment Vehicle not found");
			// }

			// var vehicle = _context.InvestmentVehicle.First(i => i.InvestmentVehicleId == vehicleId);
			// var modelList = new List<InvestmentModel>();

			// if(vehicle.Investments.Any()){
			// 	foreach(var investment in vehicle.Investments){
			// 		if(investment.InvestmentModel is not null){
			// 			modelList.Add(investment.InvestmentModel);
			// 		} else {
			// 			var model = investment.InvokeAnalysis(options ?? new OptionsDict());
			// 			investment.InvestmentModel = model;
			// 			modelList.Add(model);
			// 		}
			// 	}
			// }

			// return modelList;

			throw new NotImplementedException();
		}

	}
}

