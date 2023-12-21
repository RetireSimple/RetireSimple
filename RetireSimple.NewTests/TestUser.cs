using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._401k;
using RetireSimple.NewEngine.New_Engine.GrowthModels;
using RetireSimple.NewEngine.New_Engine.GrowthModels._401kGrowthModels;
using RetireSimple.NewEngine.New_Engine.TaxModels;
using RetireSimple.NewEngine.New_Engine.User;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewTests {
	[TestClass]
	public class TestUser {
		[TestMethod]
		public void TestUserGenerateProjections() {
			User user = new User(new UserInfo(21, 65, 2000000));

			user.AddInvestmentVehicle(new _401k(new NullTax(), 100, 1, new FixedGrowth(.05)));

			user.AddInvestmentVehicle(new _401k(new NullTax(), 133, 2, new FixedGrowth(.02)));

			user.AddInvestmentVehicle(new _401k(new NullTax(), 90, 3, new FixedGrowth(.1)));

			Projection projection1 = user.GenerateProjections();


			projection1.yearly_projections[9].Equals(558.45);

		}


		[TestMethod]
		public void Test401kGrowth() {

			User user = new User(new UserInfo(30, 65, 1000000));

			_401kGrowth growthModel = new _401kGrowth();

			_401kInfo info = new _401kInfo(.1, 40000, 0, .07, .5, .06);

			growthModel.SetInfo(info);

			_401k _401K = new _401k(new NullTax(), 1000, 1, growthModel);

			user.AddInvestmentVehicle(_401K);

			Projection projection = user.GenerateProjections();

			projection.yearly_projections[34].Equals(756476.60);


		}
	}
}
