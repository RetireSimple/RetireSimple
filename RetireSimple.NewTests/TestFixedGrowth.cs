using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._401k;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;
using RetireSimple.NewEngine.New_Engine.GrowthModels;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewTests {
	[TestClass]
	public class TestFixedGrowth {
		[TestMethod]
		public void TestFixedGrowth1() {

			_401k my401k = new _401k(new NullTax(), 100, 1, new FixedGrowth(.05), new NullInfo());


			Projection myProjection = my401k.Calculate(10);


			List<double> list = new List<double>();

			list.Add(100.00);
			list.Add(105.00);
			list.Add(110.25);
			list.Add(115.76);
			list.Add(121.55);
			list.Add(127.63);
			list.Add(134.01);
			list.Add(140.71);
			list.Add(147.75);
			list.Add(155.13);
			list.Add(162.89);

			myProjection.yearly_projections.Equals(list);


		}


	}
}
