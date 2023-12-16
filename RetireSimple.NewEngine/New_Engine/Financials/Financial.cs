using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.TaxModels;

namespace RetireSimple.NewEngine.New_Engine.Financials {

	public abstract class Financial {

		private int id;

		private ITax tax;

		abstract public Projection Calculate(int years);
	}

}