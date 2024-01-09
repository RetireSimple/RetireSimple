using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.GrowthModels {
	public class FixedGrowth : IGrowthModel {

		private double rate;

		public FixedGrowth(double rate) {
			this.rate = rate;
		}
		public Projection GenerateProjection(double value, int years, InvestmentVehicleInfo info) {
			List<double> values = new List<double>();

			values.Add(value);

			for (int i =1; i < years; i++) {
				double lastVal = values[i-1];

				values.Add(lastVal * (1 + this.rate));

			}

			return new Projection(values, 0);

		}
	}
}
