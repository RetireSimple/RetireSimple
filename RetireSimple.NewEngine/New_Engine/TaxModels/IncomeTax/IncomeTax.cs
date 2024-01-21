using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.TaxModels.IncomeTax.TaxBrackets;
using RetireSimple.NewEngine.New_Engine.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.TaxModels.IncomeTax {
	public class IncomeTax : ITax {


		private IncomeBrackets brackets;
		private UserTaxStatus status;

		public IncomeTax(IncomeBrackets brackets, UserTaxStatus status) {
			this.brackets = brackets;
			this.status = status;

		}
		public double CalculateTax(double income) {

			var taxRate = brackets.GetTaxRate(income, this.status);

			return income * (1-taxRate);

		}
	}
}
