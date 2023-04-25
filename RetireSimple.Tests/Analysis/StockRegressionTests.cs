using FluentAssertions.Execution;

using MathNet.Numerics.Distributions;

using Microsoft.Extensions.Options;

using Moq;

using RetireSimple.Engine.Analysis.Presets;
using RetireSimple.Engine.Analysis.Utils;
using RetireSimple.Engine.Data.Analysis;
using RetireSimple.Engine.Data.Base;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static RetireSimple.Engine.Analysis.Utils.Regression;

namespace RetireSimple.Tests.Analysis {

	public class StockRegressionTests {
		public OptionsDict TestOptions { get; set; } = new OptionsDict();

		public StockRegressionTests() {
			TestOptions["stockQuantity"] = "5";
			TestOptions["basePrice"] = "5";
			TestOptions["analysisLength"] = "5";
			TestOptions["uncertainty"] = "0.25";
			TestOptions["percentGrowth"] = "0.12";
		}

		InvestmentModel expected = new InvestmentModel() {
			MinModelData = { 19, 24, 13, 13, 10 },
			AvgModelData = { 25, 40, 29, 40, 44 },
			MaxModelData = { 30, 56, 45, 67, 78 },
		};

		[Fact]
		public void RegressionTestBasic() {
			var reg = new Regression(TestOptions);
			var actual = reg.RunSimulation();
			for(int i =0; i < int.Parse(TestOptions["analysisLength"]); i++) {
				Assert.Equal(actual.MinModelData[i].ToString("#"), expected.MinModelData[i].ToString("#"));
				Assert.Equal(actual.AvgModelData[i].ToString("#"), expected.AvgModelData[i].ToString("#"));
				Assert.Equal(actual.MaxModelData[i].ToString("#"), expected.MaxModelData[i].ToString("#"));
			}
			//actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void RegressionTestRegSim() {

			var reg = new Regression(TestOptions);
			var actual = reg.RunSimulation();
			for (int i = 0; i < int.Parse(TestOptions["analysisLength"]); i++) {
				Assert.Equal(actual.MinModelData[i].ToString("#"), expected.MinModelData[i].ToString("#"));
				Assert.Equal(actual.AvgModelData[i].ToString("#"), expected.AvgModelData[i].ToString("#"));
				Assert.Equal(actual.MaxModelData[i].ToString("#"), expected.MaxModelData[i].ToString("#"));
			}
			//actual.Should().BeEquivalentTo(expected);
		}
	}
}
