namespace RetireSimple.Tests.Analysis {

	public class VehicleAnalysisTests {

		[Theory,
		MemberData(nameof(VehicleAnalysisTestData.CashSimVars),
					MemberType = typeof(VehicleAnalysisTestData))]
		public void SimulateCashContributions_DefaultAfterTaxReturnsValues(OptionsDict options, decimal initialHoldings, List<decimal> expected) {
			var vehicle = new Vehicle401k { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };

			var result = vehicle.SimulateCashContributions(options);

			result.Should().HaveSameCount(expected);
			result.Should().BeEquivalentTo(expected);
		}

		[Theory,
		MemberData(nameof(VehicleAnalysisTestData.CashSimVars_PostTax),
					MemberType = typeof(VehicleAnalysisTestData))]
		public void SimulateCashContributions_DefaultPreTaxReturnsValues(OptionsDict options, decimal initialHoldings, List<decimal> expected) {
			var vehicle = new VehicleRothIRA { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };

			var result = vehicle.SimulateCashContributions(options);

			result.Should().HaveSameCount(expected);
			result.Should().BeEquivalentTo(expected);
		}

		[Theory,
		MemberData(nameof(VehicleAnalysisTestData.AggregateSimVars_PreTax),
					MemberType = typeof(VehicleAnalysisTestData))]
		public void GeneratePreTaxModel_DefaultAfterTaxReturnsValues(List<InvestmentModel> models,
																	OptionsDict options,
																	decimal initialHoldings,
																	InvestmentModel expected) {
			var vehicle = new Vehicle401k { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
			var combinedOptions = vehicle.MergeOverrideOptions(options);    //Done in the GenerateAnalysis() call
			var cashSim = VehicleDefaultAS.SimulateCashContributionsDefaultAfterTax(vehicle, combinedOptions);

			var result = VehicleDefaultAS.GeneratePreTaxModelDefaultAfterTaxVehicle(combinedOptions, models, cashSim);

			result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
			result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
			result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		}


		[Theory,
		MemberData(nameof(VehicleAnalysisTestData.AggregateSimVars_PostTax),
			MemberType = typeof(VehicleAnalysisTestData))]
		public void GeneratePostTaxModel_DefaultAfterTaxReturnsValues(List<InvestmentModel> models,
																	OptionsDict options,
																	decimal initialHoldings,
																	InvestmentModel expected) {
			var vehicle = new Vehicle401k { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
			var combinedOptions = vehicle.MergeOverrideOptions(options);
			var cashSim = VehicleDefaultAS.SimulateCashContributionsDefaultAfterTax(vehicle, combinedOptions);

			var result = VehicleDefaultAS.GeneratePostTaxModelDefaultAfterTaxVehicle(combinedOptions, models, cashSim);

			result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
			result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
			result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		}


		[Theory(Skip = "Needs Fixing"),
		MemberData(nameof(VehicleAnalysisTestData.AggregateSimVarsPreTaxVehicle_PreTax),
					MemberType = typeof(VehicleAnalysisTestData))]
		public void GeneratePreTaxModel_DefaultPreTaxReturnsValues(List<InvestmentModel> models,
																	OptionsDict options,
																	decimal initialHoldings,
																	InvestmentModel expected) {
			var vehicle = new Vehicle401k { InvestmentVehicleData = new OptionsDict() { ["cashHoldings"] = initialHoldings.ToString() } };
			var combinedOptions = vehicle.MergeOverrideOptions(options);
			var cashSim = VehicleDefaultAS.SimulateCashContributionsDefaultAfterTax(vehicle, combinedOptions);

			var result = VehicleDefaultAS.GeneratePreTaxModelDefaultPreTaxVehicle(combinedOptions, models, cashSim);

			result.MinModelData.Should().BeEquivalentTo(expected.MinModelData);
			result.AvgModelData.Should().BeEquivalentTo(expected.AvgModelData);
			result.MaxModelData.Should().BeEquivalentTo(expected.MaxModelData);
		}
	}

	static class VehicleAnalysisTestData {
		/********************************************************
		 * Defaults + Mock Data
		 ********************************************************/
		public static readonly OptionsDict DefaultOptions = new() {
			["analysisLength"] = "10",
			["cashContribution"] = "0",
			["vehicleTaxPercentage"] = "0.3",
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

		public static readonly List<InvestmentModel> ExpectedMockModels = new() {
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


		/********************************************************
		 * 		 Test Parameters
		 ********************************************************/
		public static readonly IEnumerable<object[]> CashSimVars = new List<object[]> {
			new object[] {
				new OptionsDict(DefaultOptions){ ["cashContribution"] = "0" },
				0.0m,
				Enumerable.Repeat(0m, 10).ToList(),
			},
			new object[] {
				new OptionsDict(DefaultOptions){ ["cashContribution"] = "10" },
				0.0m,
				new List<decimal>(){0, 10, 20, 30, 40, 50, 60, 70, 80, 90}
			},
			new object[] {
				new OptionsDict(DefaultOptions){ ["cashContribution"] = "0" },
				100.0m,
				Enumerable.Repeat(100m, 10).ToList()
			},
			new object[] {
				new OptionsDict(DefaultOptions){ ["cashContribution"] = "10"},
				100.0m,
				new List<decimal>(){100m, 110m, 120m, 130m, 140m, 150m, 160m, 170m, 180m, 190m}
			}
		};

		public static readonly IEnumerable<object[]> CashSimVars_PostTax = new List<object[]> {
			new object[] { new OptionsDict(DefaultOptions){ ["cashContribution"] = "0" }, 0.0m,
				new List<decimal>(){0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
			},
			new object[] { new OptionsDict(DefaultOptions){ ["cashContribution"] = "10" }, 0.0m,
				new List<decimal>(){0, 7,14, 21, 28, 35, 42, 49, 56, 63}
			},
			new object[] { new OptionsDict(DefaultOptions){ ["cashContribution"] = "0" }, 100.0m,
				new List<decimal>(){100m, 100m, 100m, 100m, 100m, 100m, 100m, 100m, 100m, 100m, }
			},
			new object[] { new OptionsDict(DefaultOptions){ ["cashContribution"] = "10" }, 100.0m,
				new List<decimal>(){100m, 107m, 114m, 121m, 128m, 135m, 142m, 149m, 156m, 163m}
			}
		};

		public static readonly IEnumerable<object[]> AggregateSimVars_PreTax = new List<object[]> {
			new object[] { MockModels.GetRange(0, 1), DefaultOptions, 0.0m, ExpectedMockModels[0] },
			new object[] { MockModels.GetRange(0, 1), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[0].MinModelData.Select(x=> x+100).ToList(),
					AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x=> x+100).ToList(),
					MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x=> x+100).ToList(),
				}
			},
			new object[] { MockModels.GetRange(0, 2), DefaultOptions, 0.0m, ExpectedMockModels[1] },
			new object[] { MockModels.GetRange(0, 2), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[1].MinModelData.Select(x=> x+100).ToList(),
					AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x=> x+100).ToList(),
					MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x=> x+100).ToList(),
				}
			},
			new object[] { MockModels.GetRange(0, 3), DefaultOptions, 0.0m, ExpectedMockModels[2] },
			new object[] { MockModels.GetRange(0, 3), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[2].MinModelData.Select(x=> x+100).ToList(),
					AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x=> x+100).ToList(),
					MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x=> x+100).ToList(),
				}
			},
			new object[] { MockModels.GetRange(0, 4), DefaultOptions, 0.0m, ExpectedMockModels[3] },
			new object[] { MockModels.GetRange(0, 4), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[3].MinModelData.Select(x=> x+100).ToList(),
					AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x=> x+100).ToList(),
					MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x=> x+100).ToList(),
				}
			},
		};

		public static readonly IEnumerable<object[]> AggregateSimVars_PostTax = new List<object[]> {
			new object[] { MockModels.GetRange(0, 1), DefaultOptions, 0.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[0].MinModelData.Select(x => x * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x => x * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x => x * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 1), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[0].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 2), DefaultOptions, 0.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[1].MinModelData.Select(x => x * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x => x * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x => x * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 2), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[1].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 3), DefaultOptions, 0.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[2].MinModelData.Select(x => x * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x => x * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x => x * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 3), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[2].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 4), DefaultOptions, 0.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[3].MinModelData.Select(x => x * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x => x * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x => x * 0.7m).ToList(),
				}},
			new object[] { MockModels.GetRange(0, 4), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[3].MinModelData.Select(x => (x+100) * 0.7m).ToList(),
					AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x => (x+100) * 0.7m).ToList(),
					MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x => (x+100) * 0.7m).ToList(),
				}},
		};

		public static readonly IEnumerable<object[]> AggregateSimVarsPreTaxVehicle_PreTax = new List<object[]> {
			new object[] { MockModels.GetRange(0, 1), DefaultOptions, 0.0m, ExpectedMockModels[0] },
			new object[] { MockModels.GetRange(0, 1), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[0].MinModelData.Select(x=> x+70).ToList(),
					AvgModelData = ExpectedMockModels[0].AvgModelData.Select(x=> x+70).ToList(),
					MaxModelData = ExpectedMockModels[0].MaxModelData.Select(x=> x+70).ToList(),
				}
			},
			new object[] { MockModels.GetRange(0, 2), DefaultOptions, 0.0m, ExpectedMockModels[1] },
			new object[] { MockModels.GetRange(0, 2), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[1].MinModelData.Select(x=> x+70).ToList(),
					AvgModelData = ExpectedMockModels[1].AvgModelData.Select(x=> x+70).ToList(),
					MaxModelData = ExpectedMockModels[1].MaxModelData.Select(x=> x+70).ToList(),
				}
			},
			new object[] { MockModels.GetRange(0, 3), DefaultOptions, 0.0m, ExpectedMockModels[2] },
			new object[] { MockModels.GetRange(0, 3), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[2].MinModelData.Select(x=> x+70).ToList(),
					AvgModelData = ExpectedMockModels[2].AvgModelData.Select(x=> x+70).ToList(),
					MaxModelData = ExpectedMockModels[2].MaxModelData.Select(x=> x+70).ToList(),
				}
			},
			new object[] { MockModels.GetRange(0, 4), DefaultOptions, 0.0m, ExpectedMockModels[3] },
			new object[] { MockModels.GetRange(0, 4), DefaultOptions, 100.0m,
				new InvestmentModel() {
					MinModelData = ExpectedMockModels[3].MinModelData.Select(x=> x+70).ToList(),
					AvgModelData = ExpectedMockModels[3].AvgModelData.Select(x=> x+70).ToList(),
					MaxModelData = ExpectedMockModels[3].MaxModelData.Select(x=> x+70).ToList(),
				}
			},
		};
	}
}


