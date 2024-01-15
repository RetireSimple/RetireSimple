using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.TaxModels {
	public class TaxBracket {

		private int lowerBound;
		private int upperBound;
		private TaxRate taxRate;


		public TaxBracket(int lowerBound, int upperBound, TaxRate taxRate)
		{
			this.lowerBound = lowerBound;
			this.upperBound = upperBound;
			this.taxRate = taxRate;
		}

		public Boolean inRange(double income) {
			return this.lowerBound <= income && this.upperBound >= income;
		}

	}
}
