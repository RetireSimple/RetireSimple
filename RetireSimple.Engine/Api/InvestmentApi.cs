using Microsoft.Extensions.Options;

using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.Data.Expense;

using System.Diagnostics;
using System.Reflection;

namespace RetireSimple.Engine.Api {
	internal interface IInvestmentApi {
		List<Investment> GetAllInvestments();
		List<Investment> GetSingularInvestments();
		Investment? GetInvestment(int id);
		int Add(string type, OptionsDict? parameters = null);
		void Remove(int id);
		void Update(int id, OptionsDict parameters);
		void UpdateAnalysisOptions(int id, OptionsDict options);
		InvestmentModel GetAnalysis(int id, OptionsDict? options = null);
		Dictionary<string, InvestmentModel> GetAllAnalysis();
	}

	public class InvestmentApi : IInvestmentApi {
		private readonly EngineDbContext _context;

		public InvestmentApi(EngineDbContext context) {
			_context = context;
		}

		/// <summary>
		/// Gets a list of all<see cref="Investment"/> objects </summary>
		/// <returns></returns>
		public List<Investment> GetAllInvestments() => _context.Investment.ToList();

		/// <summary>
		/// Gets a list of all<see cref="Investment"/> objects that are not
		/// associated with an <see cref="Data.Base.InvestmentVehicle"/>.
		/// </summary>
		/// <returns></returns>
		public List<Investment> GetSingularInvestments() {
			var vehicleInvestments = _context.Portfolio.First()
											.InvestmentVehicles
											.SelectMany(v => v.Investments);

			return _context.Portfolio.First()
					.Investments
					.Except(vehicleInvestments).ToList();
		}

		/// <summary>
		/// Gets a single investment by its id. Returns null if no investment with the specified id exists.
		/// </summary>
		/// <param name="id"></param>
		/// <returns>The investment </returns>
		public Investment? GetInvestment(int id) => _context.Investment.Find(id);

		/// <summary>
		/// Adds a new investment of the specified type, with investment-specific parameters set
		/// to the respective keys in <paramref name="parameters"/>. The analysis used can also be
		/// set by adding the key-value pair <code>["AnalysisType"] = [analysisNameString]</code>
		/// in <paramref name="parameters"/>.<br/> If <paramref name="parameters"/> does not contain
		/// a key for an investment-specifc parameter or for the type of analysis to use, the default
		/// value defined by the investment for that parameter is used. <br/> If <paramref name="parameters"/>
		/// is empty or null, the analysis used and other investment-specific parameters will be
		/// set to their default values.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="parameters">If unused, automatically uses internal defaults for creating the investment</param>
		/// <returns>ID of the newly created investment after it is saved in the database</returns>
		/// <exception cref="ArgumentException">The specified investment type was not found</exception>
		public int Add(string type, OptionsDict? parameters = null) {
			parameters ??= new OptionsDict();
			var moduleType = typeof(Investment).Assembly.GetTypes().First(x => x.Name == type)
				?? throw new ArgumentException($"There is no defined investment module called {type}");
			var moduleCtor = moduleType.GetConstructor(new[] { typeof(OptionsDict) })
				?? throw new InvalidOperationException($"The investment module {type} is missing a required constructor. Please recompile the Engine after fixing this issue");

			var newInvestment = moduleCtor.Invoke(new[] { parameters }) as Investment
			?? throw new InvalidOperationException($"The constructor used to construct {type} returned null");

			// Investment newInvestment = type switch {
			// 	"StockInvestment" => InvestmentApiUtil.CreateStock(parameters),
			// 	"BondInvestment" => InvestmentApiUtil.CreateBond(parameters),
			// 	_ => throw new ArgumentException("The specified investment type was not found"),
			// };
			newInvestment.InvestmentName = parameters.GetValueOrDefault("investmentName", "");


			var mainPortfolio = _context.Portfolio.First(p => p.PortfolioId == 1);
			mainPortfolio.Investments.Add(newInvestment);

			_context.SaveChanges();

			return _context.Investment.First(i => i.Equals(newInvestment)).InvestmentId;
		}

		/// <summary>
		/// Removes the investment with the specified ID. If the specified investment has any
		/// <see cref="InvestmentModel"/> or <see cref="Expense"/>
		/// associated with it, those are deleted as well. <strong>However</strong>, if there are any
		/// <see cref="InvestmentTransfer"/> objects related to this investment in any form, an
		/// <see cref="InvalidOperationException"/> is thrown.
		/// </summary>
		/// <param name="id"></param>
		/// <exception cref="ArgumentException">If the specified investment does not exist</exception>
		/// <exception cref="InvalidOperationException">If the remove occurs while the investment still has
		/// some <see cref="InvestmentTransfer"/> objects related to it.</exception>
		public void Remove(int id) {
			if (!_context.Investment.Any(i => i.InvestmentId == id)) {
				throw new ArgumentException("Investment not found");
			}
			if (_context.InvestmentTransfer.Any(t => t.SourceInvestmentId == id
												|| t.DestinationInvestmentId == id)) {
				throw new InvalidOperationException("Cannot remove investment while it still has transfers");
			}

			_context.Investment.Remove(_context.Investment.First(i => i.InvestmentId == id));
			_context.SaveChanges();
		}

		/// <summary>
		/// Updates many parameters in a specific investment based on the values defined in <paramref name="parameters"/>
		/// For simplicity, this method does not validate if the specified investment has such a parameter,
		/// rather it is inserted/overwritten directly to the internal <see cref="Investment.InvestmentData"/>
		/// Values cannot be null, and keys will not be removable from this dictionary.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="parameters"></param>
		/// <exception cref="ArgumentException">If the specified investment does not exist</exception>
		public void Update(int id, OptionsDict parameters) {
			if (!_context.Investment.Any(i => i.InvestmentId == id)) {
				throw new ArgumentException("Investment not found");
			}

			var investment = _context.Investment.First(i => i.InvestmentId == id);
			//Only investmentName should be set via Property
			if (parameters.ContainsKey("investmentName")) {
				investment.InvestmentName = parameters["investmentName"];
				parameters.Remove("investmentName");
			}

			if (parameters.ContainsKey("analysisType")) {
				investment.AnalysisType = parameters["analysisType"];
				parameters.Remove("AnalysisType");
			}

			foreach (var (key, value) in parameters) {
				//Only update existing keys
				if (investment.InvestmentData.ContainsKey(key)) {
					investment.InvestmentData[key] = value;
				}
			}

			investment.LastUpdated = DateTime.Now;

			_context.SaveChanges();
		}

		/// <summary>
		/// Updates the <see cref="Investment.AnalysisOptionsOverrides"/> with the
		/// specified analysis option and option value. No actual validation of the value with
		/// respect to the analysis option is performed in this method.
		/// If a key exists in <paramref name="options"/> and also exists in the current overrides,
		/// the value is overwritten. If a key in <paramref name="options"/>
		/// has its value as an empty string, the key is removed from the overrides if the key exists.
		/// If a key in <paramref name="options"/> does not exist in the current overrides, it is added.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="options"></param>
		public void UpdateAnalysisOptions(int id, OptionsDict options) {
			if (!_context.Investment.Any(i => i.InvestmentId == id)) {
				throw new ArgumentException("Investment not found");
			}

			var investment = _context.Investment.First(i => i.InvestmentId == id);
			foreach (var (key, value) in options) {
				if (value == "") {
					investment.AnalysisOptionsOverrides.Remove(key);
				} else {
					investment.AnalysisOptionsOverrides[key] = value;
				}
			}
			investment.LastUpdated = DateTime.Now;

			_context.SaveChanges();
		}

		/// <summary>
		/// Returns a model of the investment based on that investment's chosen analysis and (optionally)
		/// any parameters that should be used as an override. If an existing <see cref="InvestmentModel"/>
		/// already exists and the investment's last change is before the time the model was generated, no analysis
		/// is executed and the existing model is returned. If <paramref name="options"/> is passed with a non-null
		/// value, the analysis is re-executed regardless if the model is up-to-date.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public InvestmentModel GetAnalysis(int id, OptionsDict? options = null) {
			if (!_context.Investment.Any(i => i.InvestmentId == id)) {
				throw new ArgumentException("Investment not found");
			}

			var investment = _context.Investment.First(i => i.InvestmentId == id);
			if ((options is not null && options.Count > 0) ||
					investment.InvestmentModel is null ||
					investment.LastUpdated > investment.InvestmentModel.LastUpdated) {
				var model = investment.InvokeAnalysis(options ?? new OptionsDict() { });
				var updateTime = DateTime.Now;
				if (investment.InvestmentModel is not null) {
					investment.InvestmentModel.MinModelData = model.MinModelData;
					investment.InvestmentModel.AvgModelData = model.AvgModelData;
					investment.InvestmentModel.MaxModelData = model.MaxModelData;
					investment.InvestmentModel.LastUpdated = updateTime;
				} else {
					model.InvestmentId = investment.InvestmentId;
					investment.InvestmentModel = model;

					model.LastUpdated = updateTime;
				}
				investment.LastUpdated = updateTime;
				_context.SaveChanges();
			}

			var tempModel = investment.InvestmentModel;
			tempModel.MinModelData = tempModel.MinModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.AvgModelData = tempModel.AvgModelData.Select(d => Math.Max(d, 0)).ToList();
			tempModel.MaxModelData = tempModel.MaxModelData.Select(d => Math.Max(d, 0)).ToList();

			return tempModel;
		}


		//TODO fully document/Update as needed
		public Dictionary<string, InvestmentModel> GetAllAnalysis() {
			var investments = _context.Investment.ToList();
			var models = new Dictionary<string, InvestmentModel>();
			foreach (var investment in investments) {
				models[investment.InvestmentName] = GetAnalysis(investment.InvestmentId);
			}

			return models;
		}

	}
}
