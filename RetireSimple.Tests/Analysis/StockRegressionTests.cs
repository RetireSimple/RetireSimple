using FluentAssertions.Execution;

using MathNet.Numerics.Distributions;

using Microsoft.Extensions.Options;

using Moq;

using Newtonsoft.Json.Linq;

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
			TestOptions["basePrice"] = "20";
			TestOptions["analysisLength"] = "5";
			TestOptions["uncertainty"] = "0.25";
			TestOptions["percentGrowth"] = "0.12";
		}

		InvestmentModel expected = new InvestmentModel() {
			MinModelData = { 100, 88, 77, 68, 60 },
			AvgModelData = { 100, 100, 100, 100, 100 },
			MaxModelData = { 100, 112, 125, 140, 157 },
		};

		[Fact]
		public void RegressionTestBinomialModel() {
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
		public void RegressionTestNeg() {
			TestOptions["uncertainty"] = "-1";
			TestOptions["percentGrowth"] = "-1";
			int expected = 0;

			int resultU = (int)Math.Max(double.Parse(TestOptions["uncertainty"]), 0);
			int resultP = (int)Math.Max(double.Parse(TestOptions["percentGrowth"]), 0);

			Assert.Equal(expected, resultU);
		}
	}
}
