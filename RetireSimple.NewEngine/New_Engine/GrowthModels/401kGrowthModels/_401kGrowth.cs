using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._401k;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.GrowthModels._401kGrowthModels {
	public class _401kGrowth : IGrowthModel {

		private _401kInfo? info;

		public _401kGrowth() {
			info = null;
		}

		public void SetInfo(_401kInfo info) {
			this.info = info;
		}




		public Projection GenerateProjection(double value, int years) {
			List<double> values = new List<double>();

			values.Add(value);

			for(int i=0; i < years; i++) {

				double personal_contribution = this.info.contributions * this.CalculateSalaryIncrease(i);

				double employer_contribution = this.CalculateEmployerContributions(i);

				double newVal = values[i] + personal_contribution + employer_contribution;

				double newVal_withGrowth = newVal * (1 + this.info.rate);

				values.Add(newVal_withGrowth);

			}

			return new Projection(values);

		}
		private double CalculateEmployerContributions(int i) {

			double contributions;


			if(this.info.contributions < this.info.employerMatchCap) {
				contributions = this.info.employerMatch * this.CalculateSalaryIncrease(i) * this.info.contributions;
			} else {
				contributions = this.info.employerMatch * this.CalculateSalaryIncrease(i) * this.info.employerMatchCap;
			}

			return contributions;


		}

		private double CalculateSalaryIncrease(int i) {
			return this.info.salary * Math.Pow((1 + this.info.salaryIncrease), i);
		}
	}
}
