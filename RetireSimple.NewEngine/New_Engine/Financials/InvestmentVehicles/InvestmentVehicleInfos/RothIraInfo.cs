using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos {
	public class RothIraInfo : InvestmentVehicleInfo {

		public double annual_contributions;

		public double rate; 


		public RothIraInfo(double annual_contributions, double rate) {
			this.annual_contributions = annual_contributions;
			this.rate = rate;
		}

	}
}
