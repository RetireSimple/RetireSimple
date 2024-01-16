namespace RetireSimple.NewEngine.New_Engine.TaxModels.TaxBrackets {
	public class TaxRange {

		public readonly int lower;
		public readonly int upper;

		//TODO making ranges open ended
		public TaxRange(int lower, int upper) {
			this.lower = lower;
			this.upper = upper;
		}

		public Boolean InRange(double val) {
			return val >= this.lower && val <= this.upper;
		}
	}
}