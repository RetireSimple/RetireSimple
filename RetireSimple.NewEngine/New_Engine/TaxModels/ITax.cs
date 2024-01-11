using RetireSimple.Engine.New_Engine;

namespace RetireSimple.NewEngine.New_Engine.TaxModels {
	public interface ITax {

		private static double totalIncome;

		public abstract Projection calculateTax(Projection projection);

		public void AddToIncome(double income) {
			ITax.totalIncome += income;
		}

		public double GetTotalIncome() { return ITax.totalIncome; }

	}

}