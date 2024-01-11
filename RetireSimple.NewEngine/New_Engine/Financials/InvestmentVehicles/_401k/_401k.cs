using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;
using RetireSimple.NewEngine.New_Engine.GrowthModels;
using RetireSimple.NewEngine.New_Engine.GrowthModels._401kGrowthModels;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._401k {
	public class _401k : InvestmentVehicle {
		public _401k(float value, int id, InvestmentVehicleInfo info) : base(id, FinCategories.INVESTMENT_VEHICLE, value, new _401kGrowth(), info) {


		}



	}
}
