using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles;
using RetireSimple.NewEngine.New_Engine.Managers;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace RetireSimple.NewEngine.New_Engine.Users {
	public class User {

		private ITax tax;

		private UserInfo userInfo;

		private Manager portfolioManager;

		public User(UserInfo userInfo) {
		
			this.userInfo = userInfo;

			this.tax = new NullTax();

			this.portfolioManager = new PortfolioManager();
		}

		public User() {
			this.userInfo = new UserInfo(30, 65, 0, UserTaxStatus.SINGLE);
			this.tax = new NullTax();
			this.portfolioManager = new PortfolioManager();
		}
	
		public void AddTax(ITax tax) {
			this.tax = tax;
		}

		public void UpdateInfo(UserInfo userInfo) {
			this.userInfo = userInfo;
		}

		public UserInfo GetInfo() {
			return this.userInfo;
		}


		public Projection GenerateProjections() {

			int years = this.userInfo.retirementAge - this.userInfo.age;

			return this.portfolioManager.Calculate(years);

		}

		public void saveToCSV(Projection proj, String test) {
			//before your loop
			var csv = new StringBuilder();

			//in your loop
			for(int i = 0; i < proj.yearly_projections.Count;i++) {
				var x = (this.userInfo.age + i).ToString();
				var y = proj.yearly_projections[i].ToString();
				//Suggestion made by KyleMit
				var newLine = x + "," + y;// string.Format("{x},{y}", x, y);
				csv.AppendLine(newLine);
			}

			//after your loop
			File.WriteAllText("..\\..\\..\\TestCSV\\" + test + ".csv", csv.ToString());
		}


		public void AddInvestmentVehicle(InvestmentVehicle vehicle) {
			this.portfolioManager.Add(vehicle);
		}

		public double ApplyTax(double income) {

			return this.tax.CalculateTax(income);
		
		}
	






	}
}
