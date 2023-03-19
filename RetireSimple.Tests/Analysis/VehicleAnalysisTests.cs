using Microsoft.Extensions.Options;

using Moq;

using Newtonsoft.Json.Linq;

using System.Security.Principal;

namespace RetireSimple.Tests.Analysis {

	public class VehicleAnalysisTests {

		[Theory, MemberData(nameof(VehicleAnalysisTestData.CashSimVars), MemberType = typeof(VehicleAnalysisTestData))]
		public void TestSimulateCashContributionsDefault(OptionsDict options, decimal initialHoldings, List<decimal> expected) {

			var vehicle = new Vehicle403b { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
			var actual = vehicle.SimulateCashContributions(options);

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void Test401kCashContributionsPercentage_WeeklyPaycheck() {

			var vehicle = new Vehicle401k { InvestmentVehicleData = VehicleAnalysisTestData.Options401k };
			var actual = vehicle.SimulateCashContributions(VehicleAnalysisTestData.Options401k);
			var expectedData = new List<decimal>() { 0, 800, 1600, 2400, 3200, 4000, 4800, 5600, 6400, 7200 };

			actual.Should().HaveSameCount(expectedData);
			actual.Should().BeEquivalentTo(expectedData);

			VehicleAnalysisTestData.Options401k["payFrequency"] = "weekly";
			vehicle = new Vehicle401k { InvestmentVehicleData = VehicleAnalysisTestData.Options401k };
			actual = vehicle.SimulateCashContributions(VehicleAnalysisTestData.Options401k);
			var expectedData2 = new List<string> { "", "738.462", "1476.923", "2215.385", "2953.846", "3692.308", "4430.769", "5169.231", "5907.692", "6646.154" };

			var newData = new List<string>();
			foreach (var data in actual) {
				newData.Add(data.ToString("#.###"));
			}
			newData.Should().HaveSameCount(expectedData2);
			newData.Should().BeEquivalentTo(expectedData2);
		}

		[Fact]
		public void Test401kCashContributionsFixed() {

			var vehicle = new Vehicle401k { InvestmentVehicleData = VehicleAnalysisTestData.Options401kFixed };
			var actual = vehicle.SimulateCashContributions(VehicleAnalysisTestData.Options401kFixed);
			var expected = new List<decimal>() { 0, 110, 220, 330, 440, 550, 660, 770, 880, 990 };

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);

			VehicleAnalysisTestData.Options401kFixed["payFrequency"] = "weekly";
			vehicle = new Vehicle401k { InvestmentVehicleData = VehicleAnalysisTestData.Options401kFixed };
			actual = vehicle.SimulateCashContributions(VehicleAnalysisTestData.Options401kFixed);
			expected = new List<decimal>() { 0, 440, 880, 1320, 1760, 2200, 2640, 3080, 3520, 3960 };

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);
		}

		//[Theory, MemberData(nameof(VehicleAnalysisTestData.AggregateSimVars_PreTax), MemberType = typeof(VehicleAnalysisTestData))]
		//public void TestGeneratePreTaxModelDefault(List<InvestmentModel> models, OptionsDict options, decimal initialHoldings, InvestmentModel expected) {

		//	var vehicle = new Vehicle403b { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
		//	var combinedOptions = vehicle.MergeOverrideOptions(options);
		//	var cashSim = VehicleDefaultAS.SimulateCashContributionsDefault(vehicle, combinedOptions);
		//	var result = VehicleDefaultAS.GeneratePreTaxModelDefault(combinedOptions, models, cashSim);

		//	result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
		//	result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
		//	result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		//}

		//[Theory, MemberData(nameof(VehicleAnalysisTestData.AggregateSimVars_PostTax), MemberType = typeof(VehicleAnalysisTestData))]
		//public void TestGeneratePostTaxModelDefault(List<InvestmentModel> models,OptionsDict options,decimal initialHoldings,InvestmentModel expected) {

		//	var vehicle = new Vehicle403b { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
		//	var combinedOptions = vehicle.MergeOverrideOptions(options);
		//	var cashSim = VehicleDefaultAS.SimulateCashContributionsDefault(vehicle, combinedOptions);
		//	var result = VehicleDefaultAS.GeneratePostTaxModelDefault(combinedOptions, models, cashSim);

		//	result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
		//	result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
		//	result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		//}

		//[Theory, MemberData(nameof(VehicleAnalysisTestData.AggregateSimVars_PostTax), MemberType = typeof(VehicleAnalysisTestData))]
		//public void TestGeneratePostTaxModel401k(List<InvestmentModel> models, OptionsDict options, decimal initialHoldings, InvestmentModel expected) {

		//	var vehicle = new Vehicle401k { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
		//	var combinedOptions = vehicle.MergeOverrideOptions(options);
		//	var cashSim = VehicleDefaultAS.SimulateCashContributionsDefault(vehicle, combinedOptions);
		//	var result = VehicleDefaultAS.GeneratePostTaxModelDefault(combinedOptions, models, cashSim);

		//	result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
		//	result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
		//	result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		//}
	}

	static class VehicleAnalysisTestData {

		public static readonly OptionsDict DefaultOptions = new() {
			["analysisLength"] = "10",
			["userContributionAmount"] = "0",
		};

		public static readonly OptionsDict Options401k = new() {
			["analysisLength"] = "10",
			["userContributionAmount"] = "0",
			["vehicleTaxPercentage"] = "0.3",
			["cashHoldings"] = "0",
			["payFrequency"] = "monthly",
			["salary"] = "60000",
			["maxEmployerContributionPercentage"] = "0.06",
			["userContributionPercentage"] = "0.1",
			["employerMatchPercentage"] = "1.2",
			["userContributionType"] = "percentage",
		};

		public static readonly OptionsDict Options401kFixed = new() {
			["analysisLength"] = "10",
			["cashHoldings"] = "0",
			["payFrequency"] = "monthly",
			["salary"] = "60000",
			["maxEmployerContributionPercentage"] = "0.06",
			["userContributionPercentage"] = "0.1",
			["employerMatchPercentage"] = "1.2",
			["userContributionType"] = "fixed",
			["userContributionAmount"] = "50",
		};

		public static readonly List<InvestmentModel> MockModels = new(){
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 100, 105, 110, 115, 120, 125, 130, 135, 140, 145 },
				AvgModelData = new List<decimal>(){ 100, 110, 120, 130, 140, 150, 160, 170, 180, 190 },
				MaxModelData = new List<decimal>(){ 100, 115, 130, 145, 160, 175, 190, 205, 220, 235 },
			},
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 },
				AvgModelData = new List<decimal>(){ 50, 60, 70, 80, 90, 100, 110, 120, 130, 140 },
				MaxModelData = new List<decimal>(){ 50, 65, 80, 95, 110, 125, 140, 155, 170, 185 },
			},
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 },
				AvgModelData = new List<decimal>(){ 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 },
				MaxModelData = new List<decimal>(){ 50, 55, 60, 65, 70, 75, 80, 85, 90, 95 },
			},
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 50, 45, 55, 35, 65, 25, 75, 15, 85, 5 },
				AvgModelData = new List<decimal>(){ 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 },
				MaxModelData = new List<decimal>(){ 50, 55, 45, 65, 35, 75, 25, 85, 15, 95 },
			},
		};

		public static readonly List<InvestmentModel> ExpectedModelsDefault = new() {
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 100, 105, 110, 115, 120, 125, 130, 135, 140, 145 },
				AvgModelData = new List<decimal>(){ 100, 110, 120, 130, 140, 150, 160, 170, 180, 190 },
				MaxModelData = new List<decimal>(){ 100, 115, 130, 145, 160, 175, 190, 205, 220, 235 },
			},
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 150, 160, 170, 180, 190, 200, 210, 220, 230, 240 },
				AvgModelData = new List<decimal>(){ 150, 170, 190, 210, 230, 250, 270, 290, 310, 330 },
				MaxModelData = new List<decimal>(){ 150, 180, 210, 240, 270, 300, 330, 360, 390, 420 },
			},
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 200, 205, 210, 215, 220, 225, 230, 235, 240, 245 },
				AvgModelData = new List<decimal>(){ 200, 220, 240, 260, 280, 300, 320, 340, 360, 380 },
				MaxModelData = new List<decimal>(){ 200, 235, 270, 305, 340, 375, 410, 445, 480, 515 },
			},
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 250, 250, 265, 250, 285, 250, 305, 250, 325, 250 },
				AvgModelData = new List<decimal>(){ 250, 270, 290, 310, 330, 350, 370, 390, 410, 430 },
				MaxModelData = new List<decimal>(){ 250, 290, 315, 370, 375, 450, 435, 530, 495, 610 },
			},
		};

		public static readonly List<InvestmentModel> ExpectedModels401k = new() {
			new InvestmentModel(){
				MinModelData = new List<decimal>(){ 0, 800, 1600, 2400, 3200, 4000, 4800, 5600, 6400, 7200 },
				AvgModelData = new List<decimal>(){ 0, 800, 1600, 2400, 3200, 4000, 4800, 5600, 6400, 7200 },
				MaxModelData = new List<decimal>(){ 0, 800, 1600, 2400, 3200, 4000, 4800, 5600, 6400, 7200 },
			},
		};


		/********************************************************
		 * 		 Test Parameters
		 ********************************************************/
		public static readonly IEnumerable<object[]> CashSimVars = new List<object[]> {
			new object[] {
				new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "0" },
				0.0m,
				Enumerable.Repeat(0m, 10).ToList(),
			},
			new object[] {
				new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "10" },
				0.0m,
				new List<decimal>(){0, 10, 20, 30, 40, 50, 60, 70, 80, 90}
			},
			new object[] {
				new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "0" },
				100.0m,
				Enumerable.Repeat(100m, 10).ToList()
			},
			new object[] {
				new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "10"},
				100.0m,
				new List<decimal>(){100m, 110m, 120m, 130m, 140m, 150m, 160m, 170m, 180m, 190m}
			}
		};

		public static readonly IEnumerable<object[]> CashSimVars_PostTax = new List<object[]> {
			new object[] { new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "0" }, 0.0m,
				new List<decimal>(){0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			},
			new object[] { new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "10" }, 0.0m,
				new List<decimal>(){0, 7,14, 21, 28, 35, 42, 49, 56, 63}
			},
			new object[] { new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "0" }, 100.0m,
				new List<decimal>(){100m, 100m, 100m, 100m, 100m, 100m, 100m, 100m, 100m, 100m, }
			},
			new object[] { new OptionsDict(DefaultOptions){ ["userContributionAmount"] = "10" }, 100.0m,
				new List<decimal>(){100m, 107m, 114m, 121m, 128m, 135m, 142m, 149m, 156m, 163m}
			}
		};

		//public static readonly IEnumerable<object[]> AggregateSimVars_PreTax = new List<object[]> {
		//	new object[] { MockModels.GetRange(0, 1), DefaultOptions, 0.0m, ExpectedMockModels[0] },
		//	new object[] { MockModels.GetRange(0, 1), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[0].MinModelData.Select(x=> x+100).ToList(),
		//			AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x=> x+100).ToList(),
		//			MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x=> x+100).ToList(),
		//		}
		//	},
		//	new object[] { MockModels.GetRange(0, 2), DefaultOptions, 0.0m, ExpectedMockModels[1] },
		//	new object[] { MockModels.GetRange(0, 2), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[1].MinModelData.Select(x=> x+100).ToList(),
		//			AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x=> x+100).ToList(),
		//			MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x=> x+100).ToList(),
		//		}
		//	},
		//	new object[] { MockModels.GetRange(0, 3), DefaultOptions, 0.0m, ExpectedMockModels[2] },
		//	new object[] { MockModels.GetRange(0, 3), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[2].MinModelData.Select(x=> x+100).ToList(),
		//			AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x=> x+100).ToList(),
		//			MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x=> x+100).ToList(),
		//		}
		//	},
		//	new object[] { MockModels.GetRange(0, 4), DefaultOptions, 0.0m, ExpectedMockModels[3] },
		//	new object[] { MockModels.GetRange(0, 4), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[3].MinModelData.Select(x=> x+100).ToList(),
		//			AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x=> x+100).ToList(),
		//			MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x=> x+100).ToList(),
		//		}
		//	},
		//};

		//public static readonly IEnumerable<object[]> AggregateSimVars_PostTax = new List<object[]> {
		//	new object[] { MockModels.GetRange(0, 1), DefaultOptions, 0.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[0].MinModelData.Select(x => x * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x => x * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x => x * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 1), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[0].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 2), DefaultOptions, 0.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[1].MinModelData.Select(x => x * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x => x * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x => x * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 2), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[1].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 3), DefaultOptions, 0.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[2].MinModelData.Select(x => x * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x => x * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x => x * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 3), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[2].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 4), DefaultOptions, 0.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[3].MinModelData.Select(x => x * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x => x * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x => x * 0.7m).ToList(),
		//		}},
		//	new object[] { MockModels.GetRange(0, 4), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[3].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
		//			MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
		//		}},
		//};

		//public static readonly IEnumerable<object[]> AggregateSimVarsPreTaxVehicle_PreTax = new List<object[]> {
		//	new object[] { MockModels.GetRange(0, 1), DefaultOptions, 0.0m, ExpectedMockModels[0] },
		//	new object[] { MockModels.GetRange(0, 1), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[0].MinModelData.Select(x=> x+70).ToList(),
		//			AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x=> x+70).ToList(),
		//			MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x=> x+70).ToList(),
		//		}
		//	},
		//	new object[] { MockModels.GetRange(0, 2), DefaultOptions, 0.0m, ExpectedMockModels[1] },
		//	new object[] { MockModels.GetRange(0, 2), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[1].MinModelData.Select(x=> x+70).ToList(),
		//			AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x=> x+70).ToList(),
		//			MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x=> x+70).ToList(),
		//		}
		//	},
		//	new object[] { MockModels.GetRange(0, 3), DefaultOptions, 0.0m, ExpectedMockModels[2] },
		//	new object[] { MockModels.GetRange(0, 3), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[2].MinModelData.Select(x=> x+70).ToList(),
		//			AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x=> x+70).ToList(),
		//			MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x=> x+70).ToList(),
		//		}
		//	},
		//	new object[] { MockModels.GetRange(0, 4), DefaultOptions, 0.0m, ExpectedMockModels[3] },
		//	new object[] { MockModels.GetRange(0, 4), DefaultOptions, 100.0m,
		//		new InvestmentModel() {
		//			MinModelData = ExpectedMockModels[3].MinModelData.Select(x=> x+70).ToList(),
		//			AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x=> x+70).ToList(),
		//			MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x=> x+70).ToList(),
		//		}
		//	},
		//};
	}
}


