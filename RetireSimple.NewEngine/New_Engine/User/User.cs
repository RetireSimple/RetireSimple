using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles;
using RetireSimple.NewEngine.New_Engine.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.User {
	public class User {


		private UserInfo userInfo;

		private Manager portfolioManager;

		public User(UserInfo userInfo) {
		
			this.userInfo = userInfo;

			this.portfolioManager = new PortfolioManager();
		}


		public Projection GenerateProjections() {

			int years = this.userInfo.retirementAge - this.userInfo.age;

			return this.portfolioManager.Calculate(years);

		}


		public void AddInvestmentVehicle(InvestmentVehicle vehicle) {
			this.portfolioManager.Add(vehicle);
		}






	}
}
