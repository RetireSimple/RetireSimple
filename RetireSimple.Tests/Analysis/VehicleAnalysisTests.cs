using MathNet.Numerics;

using Moq;

using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

namespace RetireSimple.Tests.Analysis {

	public class VehicleAnalysisTests {

		[Fact]
		public void GenerateAnalysis_TemplateMethodCallsAbstractInOrder() {
			var mockVehicle = new Mock<InvestmentVehicle>();
			mockVehicle.Setup(x => x.GenerateAnalysis(It.IsAny<OptionsDict>())).CallBase();
			mockVehicle.Object.InvestmentVehicleId = 1;
			mockVehicle.Setup(x => x.GetContainedInvestmentModels(It.IsAny<OptionsDict>()))
						.Returns(new List<InvestmentModel>());
			mockVehicle.Setup(x => x.SimulateCashContributions(It.IsAny<OptionsDict>()))
						.Returns(new List<decimal>());
			mockVehicle.Setup(x => x.GeneratePreTaxModels(It.IsAny<OptionsDict>(),
														It.IsAny<List<InvestmentModel>>(),
														It.IsAny<List<decimal>>()))
						.Returns(new InvestmentModel());
			mockVehicle.Setup(x => x.GeneratePostTaxModels(It.IsAny<OptionsDict>(),
															It.IsAny<List<InvestmentModel>>(),
															It.IsAny<List<decimal>>()))
						.Returns(new InvestmentModel());

			var actual = mockVehicle.Object.GenerateAnalysis(new OptionsDict());

			actual.Should().BeOfType(typeof(VehicleModel));
			actual.InvestmentVehicleId.Should().Be(1);

			mockVehicle.Verify(x => x.GetContainedInvestmentModels(It.IsAny<OptionsDict>()), Times.Once);
			mockVehicle.Verify(x => x.SimulateCashContributions(It.IsAny<OptionsDict>()), Times.Once);
			mockVehicle.Verify(x => x.GeneratePreTaxModels(It.IsAny<OptionsDict>(),
															It.IsAny<List<InvestmentModel>>(),
															It.IsAny<List<decimal>>()), Times.Once);
			mockVehicle.Verify(x => x.GeneratePostTaxModels(It.IsAny<OptionsDict>(),
															It.IsAny<List<InvestmentModel>>(),
															It.IsAny<List<decimal>>()), Times.Once);

		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.CashSimVars), MemberType = typeof(VehicleAnalysisTestData))]
		public void TestSimulateCashContributionsDefault(OptionsDict options, decimal initialHoldings, List<decimal> expected) {

			var vehicle = new VehicleIRA { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
			var actual = vehicle.SimulateCashContributions(options);

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.VechicleTypes401kLike), MemberType = typeof(VehicleAnalysisTestData))]
		public void Test401kCashContributionsPercentage_MonthlyPaycheck(Type vehicleType) {
			var vehicle = Activator.CreateInstance(vehicleType) as InvestmentVehicle
					?? throw new InvalidOperationException("Could not create instance of type " + vehicleType.Name);
			vehicle.InvestmentVehicleData = VehicleAnalysisTestData.Options401k;
			var actual = vehicle.SimulateCashContributions(VehicleAnalysisTestData.Options401k);
			var expectedData = new List<decimal>() { 0, 506, 1012, 1518, 2024, 2530, 3036, 3542, 4048, 4554 };

			actual.Should().HaveSameCount(expectedData);
			actual.Should().BeEquivalentTo(expectedData);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.VechicleTypes401kLike), MemberType = typeof(VehicleAnalysisTestData))]
		public void Test401kCashContributionsPercentage_WeeklyPaycheck(Type vehicleType) {
			var testDict = new OptionsDict(VehicleAnalysisTestData.Options401k) {
				["payFrequency"] = "weekly"
			};
			var vehicle = Activator.CreateInstance(vehicleType) as InvestmentVehicle
					?? throw new InvalidOperationException("Could not create instance of type " + vehicleType.Name);
			vehicle.InvestmentVehicleData = testDict;
			var actual = vehicle.SimulateCashContributions(testDict).Select(x => x.Round(2)).ToList();
			var expectedData = new List<decimal> { 0, 467.08M, 934.15M, 1401.23M, 1868.31M, 2335.38M, 2802.46M, 3269.54M, 3736.62M, 4203.69M };

			actual.Should().HaveSameCount(expectedData);
			actual.Should().BeEquivalentTo(expectedData);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.VechicleTypes401kLike), MemberType = typeof(VehicleAnalysisTestData))]
		public void Test401kCashContributionsPercentage_BiWeeklyPaycheck(Type vehicleType) {
			var testDict = new OptionsDict(VehicleAnalysisTestData.Options401k) {
				["payFrequency"] = "biweekly"
			};
			var vehicle = Activator.CreateInstance(vehicleType) as InvestmentVehicle
					?? throw new InvalidOperationException("Could not create instance of type " + vehicleType.Name);
			vehicle.InvestmentVehicleData = testDict;
			var actual = vehicle.SimulateCashContributions(testDict).Select(x => x.Round(2)).ToList();
			var expectedData = new List<decimal>() { 0, 467.08M, 934.15M, 1401.23M, 1868.31M, 2335.38M, 2802.46M, 3269.54M, 3736.62M, 4203.69M };

			actual.Should().HaveSameCount(expectedData);
			actual.Should().BeEquivalentTo(expectedData);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.VechicleTypes401kLike), MemberType = typeof(VehicleAnalysisTestData))]
		public void Test401kCashContributionsFixed_MonthlyPaycheck(Type vehicleType) {
			var vehicle = Activator.CreateInstance(vehicleType) as InvestmentVehicle
					?? throw new InvalidOperationException("Could not create instance of type " + vehicleType.Name);
			vehicle.InvestmentVehicleData = VehicleAnalysisTestData.Options401k;
			var expected = new List<decimal>() { 0, 50.6M, 101.2M, 151.8M, 202.4M, 253M, 303.6M, 354.2M, 404.8M, 455.4M };

			var actual = vehicle.SimulateCashContributions(VehicleAnalysisTestData.Options401kFixed);

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.VechicleTypes401kLike), MemberType = typeof(VehicleAnalysisTestData))]
		public void Test401kCashContributionsFixed_WeeklyPaycheck(Type vehicleType) {
			var testDict = new OptionsDict(VehicleAnalysisTestData.Options401kFixed) {
				["payFrequency"] = "weekly"
			};
			var expected = new List<decimal>() { 0, 202.4M, 404.8M, 607.2M, 809.6M, 1012M, 1214.4M, 1416.8M, 1619.2M, 1821.6M };
			var vehicle = Activator.CreateInstance(vehicleType) as InvestmentVehicle
					?? throw new InvalidOperationException("Could not create instance of type " + vehicleType.Name);
			vehicle.InvestmentVehicleData = testDict;

			var actual = vehicle.SimulateCashContributions(testDict);

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.VechicleTypes401kLike), MemberType = typeof(VehicleAnalysisTestData))]
		public void Test401kCashContributionsFixed_BiWeeklyPaycheck(Type vehicleType) {
			var testDict = new OptionsDict(VehicleAnalysisTestData.Options401kFixed) {
				["payFrequency"] = "biweekly"
			};
			var expected = new List<decimal>() { 0, 101.2M, 202.4M, 303.6M, 404.8M, 506M, 607.2M, 708.4M, 809.6M, 910.8M };
			var vehicle = Activator.CreateInstance(vehicleType) as InvestmentVehicle
					?? throw new InvalidOperationException("Could not create instance of type " + vehicleType.Name);
			vehicle.InvestmentVehicleData = testDict;

			var actual = vehicle.SimulateCashContributions(testDict);

			actual.Should().HaveSameCount(expected);
			actual.Should().BeEquivalentTo(expected);
		}

		[Theory, MemberData(nameof(VehicleAnalysisTestData.AggregateSimVars_PreTax), MemberType = typeof(VehicleAnalysisTestData))]
		public void TestGeneratePreTaxModelDefault(List<InvestmentModel> models, OptionsDict options, decimal initialHoldings, InvestmentModel expected) {
			var vehicle = new Vehicle403b { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
			var combinedOptions = vehicle.MergeOverrideOptions(options);
			var cashSim = VehicleDefaultAS.SimulateCashContributionsDefault(vehicle, combinedOptions);
			var result = VehicleDefaultAS.GeneratePreTaxModelDefault(combinedOptions, models, cashSim);
			result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
			result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
			result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		}
	}

	static class VehicleAnalysisTestData {
		public static readonly IEnumerable<object[]> VechicleTypes401kLike = new List<object[]>{
			new object[]{typeof(Vehicle401k)},
			new object[]{typeof(Vehicle403b)},
			new object[]{typeof(Vehicle457)},
		};

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
			["userContributionPercentage"] = "0.1",
			["employerMatchPercentage"] = "0.012",
			["userContributionType"] = "percentage",
		};

		public static readonly OptionsDict Options401kFixed = new() {
			["analysisLength"] = "10",
			["cashHoldings"] = "0",
			["payFrequency"] = "monthly",
			["salary"] = "60000",
			["userContributionPercentage"] = "0.1",
			["employerMatchPercentage"] = "0.012",
			["userContributionType"] = "fixed",
			["userContributionAmount"] = "50",
		};

		public static readonly List<InvestmentModel> ExpectedMockModels = new(){
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

		public static readonly IEnumerable<object[]> AggregateSimVars_PreTax = new List<object[]> {
			new object[] { ExpectedMockModels.GetRange(0, 1), DefaultOptions, 0.0m, ExpectedMockModels[0] },

		new object[] { ExpectedMockModels.GetRange(0, 1), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[0].MinModelData.Select(x => x + 100).ToList(),
					AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x => x + 100).ToList(),
					MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x => x + 100).ToList(),
				}
			},
		};
	}
}


