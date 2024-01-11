using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.TaxModels;

namespace RetireSimple.NewEngine.New_Engine.Financials {

	public abstract class Financial {

		private int id;

		public readonly FinCategories category;

		public Financial(int id, FinCategories category) {
			this.category = category;
			this.id = id;

		}


		abstract public Projection Calculate(int years);
	}

}