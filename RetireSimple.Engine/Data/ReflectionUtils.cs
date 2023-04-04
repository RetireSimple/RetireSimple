using Microsoft.Extensions.Options;

using RetireSimple.Engine.Data.Base;

using System.Diagnostics;

namespace RetireSimple.Engine.Data {


	public static class ReflectionUtils {
		public static Dictionary<string, Delegate> GetAnalysisModules(string investmentModule) {
			List<Delegate> modules = new List<Delegate>();

			var types = typeof(Base.Investment).Assembly.GetTypes();
			var analysisModules = types.SelectMany(t => t.GetMethods())
										.Where(m => m.GetCustomAttributes(typeof(AnalysisModuleAttribute), false).Length > 0);
			foreach (var module in analysisModules) {
				var analysisModule = module.GetCustomAttributes(typeof(AnalysisModuleAttribute), false)[0] as AnalysisModuleAttribute;
				if (analysisModule is not null &&
					analysisModule.InvestmentModule == investmentModule) {
					Console.WriteLine($"Found analysis module {module.Name} for investment module {investmentModule}");
					var delTypeParam = Type.GetType($"RetireSimple.Engine.Data.Investment.{investmentModule}")
					?? throw new ArgumentException($"Investment Module {investmentModule} does not exist during reflection");

					var del = Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(delTypeParam), module);
					modules.Add(del);
				}
			}

			//Convert to dictionary
			Dictionary<string, Delegate> moduleDict = new Dictionary<string, Delegate>();
			foreach (var module in modules) {
				var analysisModule = module.Method.GetCustomAttributes(typeof(AnalysisModuleAttribute), false)[0] as AnalysisModuleAttribute;
				if (analysisModule is not null) {
					moduleDict.Add(module.Method.Name, module);
				}
			}

			return moduleDict;
		}

		public static void SetAnalysisModuleDelegate<T>(T investment, Delegate? del) where T : Base.Investment {
			//Check if the attribute on the investment is properly defined
			var moduleAttribute = investment.GetType().GetCustomAttributes(typeof(InvestmentModuleAttribute), false)[0] as InvestmentModuleAttribute
				?? throw new ArgumentException($"Investment Module {investment.GetType().Name} does not have a valid InvestmentModuleAttribute");
			Console.WriteLine($"AnalysisModuleField: {moduleAttribute.AnalysisModuleField}");

			typeof(T).GetProperty(moduleAttribute.AnalysisModuleField)?.SetValue(investment, del);
		}

		public static List<Type> GetInvestmentModules() {
			var types = typeof(Base.Investment).Assembly.GetTypes();
			var investmentModules = types.Where(t => t.GetCustomAttributes(typeof(InvestmentModuleAttribute), false).Length > 0);
			return investmentModules.ToList();
		}


		public static List<Type> GetInvestmentVehicleModules() {
			var types = typeof(Base.Investment).Assembly.GetTypes();
			var vehicleModules = types.Where(t => t.GetCustomAttributes(typeof(InvestmentVehicleModuleAttribute), false).Length > 0);

			return vehicleModules.ToList();
		}

		public static Dictionary<string, OptionsDict> GetAnalysisPresets() {
			var callingModule = new StackTrace()?.GetFrame(1)?.GetMethod()?.DeclaringType?.Name
								?? throw new ArgumentNullException("Unable to determine calling module during preset resolution");
			var types = typeof(Base.Investment).Assembly.GetTypes();
			var presets = types.SelectMany(t => t.GetFields())
								.Where(t => t.GetCustomAttributes(typeof(AnalysisPresetAttribute), false).Length > 0);
			var presetDict = new Dictionary<string, OptionsDict>();

			foreach (var preset in presets) {
				var presetAttribute = preset.GetCustomAttributes(typeof(AnalysisPresetAttribute), false)[0] as AnalysisPresetAttribute;
				if (presetAttribute is not null) {
					if (presetAttribute.SupportedModules.Contains(callingModule)) {
						presetDict.Add(preset.Name,
										preset.GetValue(null) as OptionsDict
											?? throw new ArgumentNullException("Preset value could not be resolved"));
					}
				}
			}

			return presetDict;
		}
	}

	/**************************************
	* Custom Attributes Used in Reflection
	**************************************/

	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class InvestmentModuleAttribute : Attribute {
		public string AnalysisModuleField { get; } = "AnalysisMethod";
		public InvestmentModuleAttribute(string moduleField) {
			AnalysisModuleField = moduleField;
		}
	}


	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class AnalysisModuleAttribute : Attribute {
		//Encapsulated Fields
		public string InvestmentModule { get; }   //The name of the investment module that this analysis module can be used with

		public AnalysisModuleAttribute(string moduleName) {
			InvestmentModule = moduleName;
		}

	}

	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class InvestmentVehicleModuleAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class AnalysisPresetAttribute : Attribute {
		public List<string> SupportedModules { get; init; }
		public AnalysisPresetAttribute(params string[] modules) {
			SupportedModules = new List<string>(modules);
		}
	}
}