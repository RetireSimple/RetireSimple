using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.GrowthModels.RothIraGrowth {
	public class RothIraGrowth : IGrowthModel {
		public Projection GenerateProjection(double value, int years, InvestmentVehicleInfo info) 
		{

			return this.DoGenerateProjection(value, years, (RothIraInfo)info);

		}

		private Projection DoGenerateProjection(double value, int years, RothIraInfo info) 
		{

			List<double> values = new List<double>();

			values.Add(value);

			for(int i = 0; i < years; i++) {

				double newVal = (values[i] + info.annual_contributions)* (1 + info.rate);

				values.Add(newVal);

			}

			return new Projection(values, 0);


		}
	}
}
