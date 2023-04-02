using RetireSimple.Engine.Data.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace RetireSimple.Tests.Misc {
	public class ReflectionTests {

#pragma warning disable CS8604 //We expect these to resolve under normal scenarios
		public static readonly IEnumerable<object[]> ReflectionTheoryData = new List<object[]>() {
			new object[]{
				new StockInvestment(""),
				Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(StockInvestment)),
										typeof(StockAS).GetMethod("MonteCarlo_NormalDist"))
			},
			new object[]{
				new StockInvestment(""),
				Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(StockInvestment)),
										typeof(StockAS).GetMethod("MonteCarlo_LogNormalDist"))
				},
			new object[]{
				new BondInvestment(""),
				Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(BondInvestment)),
										typeof(BondAS).GetMethod("StdBondValuation"))
			},
		};
#pragma warning restore CS8604

		[Theory, MemberData(nameof(ReflectionTheoryData))]
		public void SetAnalysisMethodDelegate_ResolvesCorrectDelegate(Investment investment, Delegate expectedDelegate) {
			//The purpose of this test is to ensure we can set a delegate via reflection
			//Check if we have a clean slate first (i.e. AnalysisMethod is null)
			var analysisMethodProp = investment.GetType().GetProperty("AnalysisMethod");
			analysisMethodProp.Should().NotBeNull();
			analysisMethodProp?.GetValue(investment).Should().BeNull();
			var type = investment.GetType();
			var delegateType = typeof(AnalysisModule<>).MakeGenericType(type);

			//Use Reflection to use the correct generic
			var genericSetMethod = typeof(ReflectionUtils).GetMethod("SetAnalysisModuleDelegate")?.MakeGenericMethod(type);

			//Set the delegate
			genericSetMethod?.Invoke(null, new object[] { investment, expectedDelegate });

			//Check if the delegate was set correctly
			analysisMethodProp?.GetValue(investment).Should().NotBeNull();
			analysisMethodProp?.GetValue(investment).Should().BeOfType(delegateType);
		}

#pragma warning disable CS8604
		public static readonly IEnumerable<object[]> ExpectedAnalysisModuleResolution = new List<object[]> {
			new object[] {"StockInvestment", new Dictionary<string,Delegate>(){
				["MonteCarlo_NormalDist"] = Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(StockInvestment)),
																	typeof(StockAS).GetMethod("MonteCarlo_NormalDist")),
				["MonteCarlo_LogNormalDist"] = Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(StockInvestment)),
																	typeof(StockAS).GetMethod("MonteCarlo_LogNormalDist"))
				}
			},
			new object[] {"BondInvestment", new Dictionary<string,Delegate>(){
				["StdBondValuation"] = Delegate.CreateDelegate(typeof(AnalysisModule<>).MakeGenericType(typeof(BondInvestment)),
																typeof(BondAS).GetMethod("StdBondValuation"))
				}
			}
		};
#pragma warning restore CS8604

		[Theory, MemberData(nameof(ExpectedAnalysisModuleResolution))]
		public void GetAnalysisModules_DoesResolveMinimumExpectations(string module, Dictionary<string, Delegate> expectedDict) {
			var actualDict = ReflectionUtils.GetAnalysisModules(module);

			actualDict.Should().NotBeNull();
			actualDict.Should().HaveCountGreaterThanOrEqualTo(expectedDict.Count);
			//Becuase we can't necessarily generate equality for the Delegate type, compare by common values we expect
			foreach (var key in expectedDict.Keys) {
				actualDict.Should().ContainKey(key);
				actualDict[key].Should().NotBeNull();
				actualDict[key].Should().BeOfType(expectedDict[key].GetType());
				actualDict[key].Method.Name.Should().Be(expectedDict[key].Method.Name);
				actualDict[key].Method.DeclaringType.Should().Be(expectedDict[key].Method.DeclaringType);
			}
		}

		[Fact]
		public void GetInvestmentModules_ReturnsKnownCorrectModules() {
			var moduleList = ReflectionUtils.GetInvestmentModules();

			moduleList.Should().NotBeNull();
			moduleList.Should().HaveCountGreaterThanOrEqualTo(2);
			moduleList.Should().Contain(typeof(StockInvestment));
			moduleList.Should().Contain(typeof(BondInvestment));
		}

		[Fact]
		public void GetInvestmentVehicleModules_ReturnsKnownCorrectModules() {
			var moduleList = ReflectionUtils.GetInvestmentVehicleModules();

			moduleList.Should().NotBeNull();
			moduleList.Should().HaveCountGreaterThanOrEqualTo(5);
			moduleList.Should().Contain((typeof(Vehicle401k), "401k"));
			moduleList.Should().Contain((typeof(Vehicle403b), "403b"));
			moduleList.Should().Contain((typeof(Vehicle457), "457"));
			moduleList.Should().Contain((typeof(VehicleIRA), "IRA"));
			moduleList.Should().Contain((typeof(VehicleRothIRA), "RothIRA"));
		}
	}


}