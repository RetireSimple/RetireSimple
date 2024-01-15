using RetireSimple.NewEngine.New_Engine.Investments;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetireSimple.NewEngine.New_Engine.Financials;

namespace RetireSimple.NewEngine.New_Engine.Managers {
	public class PortfolioManager : Manager {


		public PortfolioManager() {
			
		}

		public override bool Add(Financial f) 
		{
			if(f.category == FinCategories.INVESTMENT_VEHICLE) {
				base.items.Add(f);
				return true;
			} 
			else { 
				return false; 
			}


		}

		public override bool DoDelete(int id) {
			base.items.RemoveAt(id);
			return true;
		}
		public override Financial DoRead(int id) {

			return base.items[id];
		}
		public override bool DoUpdate(Financial f, int id) {
			base.items[id] = f;
			return true;
		}
	}
}
