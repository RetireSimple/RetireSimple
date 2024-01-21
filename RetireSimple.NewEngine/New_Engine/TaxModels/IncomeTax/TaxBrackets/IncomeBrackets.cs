using RetireSimple.NewEngine.New_Engine.Users;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.TaxModels.IncomeTax.TaxBrackets {
	public class IncomeBrackets {


		private List<double> rates;
		private List<TaxCategory> taxCategories;


		public IncomeBrackets(List<double> rates, List<TaxCategory> taxCategories) {
			this.rates = rates;
			this.taxCategories = taxCategories;
		}




		public double GetTaxRate(double income, UserTaxStatus name) {

			foreach (TaxCategory taxCat in taxCategories) {

				if (taxCat.name == name) {
					var index = taxCat.GetTaxRange(income);

					return rates[index];
				}

			}
			return 0;


		}




	}
}
