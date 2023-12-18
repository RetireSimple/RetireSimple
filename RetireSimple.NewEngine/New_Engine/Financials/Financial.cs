using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.TaxModels;

namespace RetireSimple.NewEngine.New_Engine.Financials {

	public abstract class Financial {

		private int id;

		private ITax tax;

		public readonly FinCategories category;

		public Financial(ITax tax, int id, FinCategories category ) {
			this.tax = tax;
			this.category = category;
			this.id = id;

		}




		abstract public Projection Calculate(int years);
	}

}