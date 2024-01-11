using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;
using RetireSimple.NewEngine.New_Engine.GrowthModels;
using RetireSimple.NewEngine.New_Engine.GrowthModels._457bGrowthModels;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._457b {
	public class _457b : InvestmentVehicle {
		public _457b(int id, float value, InvestmentVehicleInfo info) : base(id, FinCategories.INVESTMENT_VEHICLE, value, new _457bGrowth() , info) {
		}
	}
}
