using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.GrowthModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles {
	public abstract class InvestmentVehicle : Financial {

		private float value;

		private IGrowthModel growthModel;

		public InvestmentVehicle(float value, IGrowthModel growthModel) 
		{
			this.value = value;
			this.growthModel = growthModel;

		}

		public override Projection calculate(int years) 
		{
			return this.growthModel.GenerateProjection(this.value, years);
		}


	}
}
