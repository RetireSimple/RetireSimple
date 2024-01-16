using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


//used this website to verify calculations https://www.annuityexpertadvice.com/calculator/401k-calculator/ 

namespace RetireSimple.NewEngine.New_Engine.GrowthModels._401kGrowthModels {
	public class _401kGrowth : IGrowthModel {

		public _401kGrowth() {
		
		}


		private Projection DoGenerateProjection(double value, int years, _401kInfo info) {
			List<double> values = new List<double>();

			values.Add(value);

			for (int i = 0; i < years; i++) {

				//percent of salary contributions * salary (considering increase)
				double personal_contribution = info.contributions * this.CalculateSalaryIncrease(info, i);
			
				//employer contributions
				double employer_contribution = this.CalculateEmployerContributions(info, i);

				double val = values[i] * (1 + info.rate);

				double newVal = val + personal_contribution + employer_contribution;

				//previous value + personal contribution + employer contributions
				//double newVal = values[i] + personal_contribution + employer_contribution;

				//value above * (1 + the projected growth rate) 
				//double newVal_withGrowth = newVal * (1 + info.rate);

				//add new value to list 
				//values.Add(newVal_withGrowth);

				values.Add(newVal);		
				}

			return new Projection(values, 0);
		}


		public Projection GenerateProjection(double value, int years, InvestmentVehicleInfo info) {
			return this.DoGenerateProjection(value, years, (_401kInfo)info);

		}
		private double CalculateEmployerContributions(_401kInfo info, int i) {

			double contributions;

			//If the percent contribution is less than the max employer match 
			if(info.contributions < info.employerMatchCap) {

				//the percentage the employer will match * thhe salary (considering increase) * the percentage of contribution
				contributions = info.employerMatch * this.CalculateSalaryIncrease(info, i) * info.contributions;
			} else {

				//the percentage the employer will match * the salary (considering increase) * the max match of the employer
				contributions = info.employerMatch * this.CalculateSalaryIncrease(info, i) * info.employerMatchCap;
			}

			return contributions;


		}

		private double CalculateSalaryIncrease(_401kInfo info, int i) {

			//base salary * (1 + salary increase rate) ^ i 
			return info.salary * Math.Pow((1 + info.salaryIncrease), i);
		}

		
	}
}
