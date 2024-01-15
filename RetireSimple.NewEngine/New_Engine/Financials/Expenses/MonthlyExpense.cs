using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.Expenses {
	public class MonthlyExpense : Expense {
		public MonthlyExpense(int id, double amount, int start) : base( id, amount, start) {
		}

		public override Projection GenerateProjection(int years) 
		{
			List<double> values = new List<double>();
			
			for(int i = 0; i < years; i++) {
				values.Add(base.amount);
			}

			return new Projection(values, base.start); 
		}
	}
}
