﻿using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.GrowthModels;
using RetireSimple.NewEngine.New_Engine.Investments;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles {
	public abstract class InvestmentVehicle : Financial {

		private float value;

		private IGrowthModel growthModel;

		private List<Investment> investments;

		

		public InvestmentVehicle(ITax tax, int id, FinCategories category, float value, IGrowthModel growthModel) : base(tax, id, FinCategories.INVESTMENT_VEHICLE)
		{
			this.value = value;
			this.growthModel = growthModel;
			this.investments = new List<Investment>();


		}

		public override Projection Calculate(int years) 
		{
			return this.growthModel.GenerateProjection(this.value, years);
		}

		public void AddInvestment(Investment investment) 
		{
			this.investments.Add(investment);	
		}


	}
}
