using RetireSimple.Engine.New_Engine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.TaxModels {
	public class NullTax : ITax {

		public NullTax() {

		}

		public Projection calculateTax(Projection projection) {
			return projection;
		}

		public double CalculateTax(double income) {
			return income;
		}
	}
}
