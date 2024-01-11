using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.TaxModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Financials.Expenses {
	public abstract class Expense : Financial {

		public double amount;
		public int start;
		public Expense(int id, double amount, int start) : base(id, FinCategories.EXPENSE) {

			this.amount = amount;
			this.start = start;

		}

		public override Projection Calculate(int years) {
			return this.GenerateProjection(years);
		}

		public abstract Projection GenerateProjection(int years);
	}
}
