using RetireSimple.Engine.New_Engine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.TaxModels {
	public class BracketTax : ITax {

		private List<TaxBracket> brackets;

	

		public BracketTax(List<int> brackets, List<TaxRate> taxRates) 
		{
			this.brackets = new List<TaxBracket>();

			for(int i =0; i < taxRates.Count; i++) {
				for(int j = 0; j < brackets.Count-1; j++) {
					TaxBracket bracket = new TaxBracket(brackets[j] + 1, brackets[j + 1], taxRates[i]);
				}
			
			}
		
		}
		public Projection calculateTax(Projection projection) {
			double totalIncome = GetTotalIncome();

		}
}
