using RetireSimple.NewEngine.New_Engine.Interfaces;

namespace RetireSimple.Engine.New_Engine {

	public abstract class Financial
    {

        private int id; 

        private ITax tax;

        abstract public Projection calculate(int years);
    }

}