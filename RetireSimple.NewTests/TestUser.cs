using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._401k;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.RothIra;
using RetireSimple.NewEngine.New_Engine.GrowthModels;
using RetireSimple.NewEngine.New_Engine.GrowthModels._401kGrowthModels;
using RetireSimple.NewEngine.New_Engine.TaxModels;
using RetireSimple.NewEngine.New_Engine.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewTests {
	[TestClass]
	public class TestUser {

		[TestMethod]
		public void Test401kGrowth1() {

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.SINGLE));

			_401kInfo info = new _401kInfo(.1, 40000, 0, .07, .5, .06);

			_401k _401K = new _401k(1000, 1, info);

			user.AddInvestmentVehicle(_401K);

			Projection projection = user.GenerateProjections();

			//user.saveToCSV(projection, "Test401kGrowth1");

			Assert.AreEqual(729508.35, Math.Round(projection.yearly_projections[35],2));


		}

		[TestMethod]
		public void Test401kGrowth2() {

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.SINGLE));


			_401kInfo info = new _401kInfo(.08, 81000, .04, .04, 1, .06);

			_401k _401K = new _401k(1000, 1, info);

			user.AddInvestmentVehicle(_401K);

			Projection projection = user.GenerateProjections();

			Assert.AreEqual(1509910.24, Math.Round(projection.yearly_projections[35], 2));

		}


		[TestMethod]
		public void Test401kGrowth3() {

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.SINGLE));

			_401kInfo info = new _401kInfo(.118, 54000, .017, .095, .517, .083);

			_401k _401K = new _401k(1000, 1, info);

			user.AddInvestmentVehicle(_401K);

			Projection projection = user.GenerateProjections();

			//user.saveToCSV(projection, "Test401kGrowth3");

			projection.yearly_projections[34].Equals(2617434.99);

			Assert.AreEqual(2492183.79, Math.Round(projection.yearly_projections[35], 2));

		}
		//TODO
		[TestMethod]
		public void TestRothIraGrowth1() {

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.SINGLE));

			RothIraInfo info = new RothIraInfo(5000, .07);

			RothIra roth = new RothIra(1, 0, info);

			user.AddInvestmentVehicle(roth);

			Projection projection = user.GenerateProjections();

			//user.saveToCSV(projection, "TestRothIraGrowth1");

			Assert.AreEqual(744567.30, Math.Round(projection.yearly_projections[34]));


		}

		//TODO
		[TestMethod]
		public void TestRothIraGrowth2() {


			User user = new User(new UserInfo(24, 68, 1000000, UserTaxStatus.SINGLE));

			RothIraInfo info = new RothIraInfo(2000, .06);

			RothIra roth = new RothIra(1, 0, info);

			user.AddInvestmentVehicle(roth);

			Projection projection = user.GenerateProjections();

			//user.saveToCSV(projection, "TestRothIraGrowth2");

			Assert.AreEqual(423487.03, Math.Round(projection.yearly_projections[44], 2));

		}





	}
}
