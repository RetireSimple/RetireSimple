using RetireSimple.NewEngine.New_Engine.Financials;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.Managers {
	public class ExpenseManager : Manager {
		public override bool Add(Financial f) {
			if(f.category == FinCategories.EXPENSE) {
				base.items.Add(f);
				return true;
			} else {
				return false;
			}
		}
		public override bool DoDelete(int id) => throw new NotImplementedException();
		public override Financial DoRead(int id) => throw new NotImplementedException();
		public override bool DoUpdate(Financial f, int id) => throw new NotImplementedException();
	}
}
