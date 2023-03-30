using RetireSimple.Engine.Data.Investment;

using System.Reflection;

namespace RetireSimple.Engine.Data {


	public static class ReflectionUtils {
		public static List<string> GetInvestmentModules() {
			List<string> modules = new List<string>();

			var types = typeof(InvestmentBase).Assembly.GetTypes();
			var investmentModules = types.Where(t => t.GetCustomAttributes(typeof(InvestmentModuleAttribute), false).Length > 0);
			foreach (var module in investmentModules) {
				modules.Add(module.Name);
			}

			return modules;
		}

		public static Dictionary<string, Delegate> GetAnalysisModules(string investmentModule) {
			List<Delegate> modules = new List<Delegate>();

			var types = typeof(InvestmentBase).Assembly.GetTypes();
			var analysisModules = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(AnalysisModuleAttribute), false).Length > 0);
			foreach (var module in analysisModules) {
				var analysisModule = module.GetCustomAttributes(typeof(AnalysisModuleAttribute), false)[0] as AnalysisModuleAttribute;
				if (analysisModule is not null &&
					analysisModule.InvestmentModule == investmentModule) {
					Console.WriteLine($"Found analysis module {module.Name} for investment module {investmentModule}");
					var delTypeParam = Type.GetType($"RetireSimple.Engine.Data.Investment.{investmentModule}");
					if (delTypeParam is null) {
						throw new ArgumentException($"Investment Module {investmentModule} does not exist during reflection");
					}
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

		public static void SetAnalysisModuleDelegate<T>(T investment, Delegate? del) where T : InvestmentBase {
			//Check if the attribute on the investment is properly defined
			if (investment.GetType().GetCustomAttributes(typeof(InvestmentModuleAttribute), false)[0] is not InvestmentModuleAttribute moduleAttribute) {
				throw new ArgumentException($"Investment Module {investment.GetType().Name} does not have a valid InvestmentModuleAttribute");
			}

			Console.WriteLine($"AnalysisModuleField: {moduleAttribute.AnalysisModuleField}");

			// if (typeof(T).GetProperty(moduleAttribute.AnalysisModuleField) is null) {
			// 	throw new ArgumentException($"Investment Module {investment.GetType().Name} does not have a valid AnalysisModuleField");
			// }

			typeof(T).GetProperty(moduleAttribute.AnalysisModuleField)?.SetValue(investment, del);
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

}