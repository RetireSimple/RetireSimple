using RetireSimple.NewEngine.New_Engine.TaxModels.TaxBrackets;
using RetireSimple.NewEngine.New_Engine.Users;

namespace RetireSimple.NewEngine.New_Engine.TaxModels.IncomeTax.TaxBrackets {
	public class TaxCategory {

		public readonly UserTaxStatus name;

		private List<TaxRange> ranges;

		public TaxCategory(UserTaxStatus name, List<TaxRange> ranges) {
			this.name = name;
			this.ranges = ranges;
		}

		public int GetTaxRange(double income) {
			return GetTaxRangeHelper(income, 0);
		}

		private int GetTaxRangeHelper(double income, int i) {
			TaxRange range = ranges[i];
			if (range.InRange(income)) {
				return i;
			} else {
				return GetTaxRangeHelper(income, i + 1);
			}
		}


	}
}