using RetireSimple.Engine.New_Engine;

namespace RetireSimple.NewEngine.New_Engine.GrowthModels {
	public interface IGrowthModel {
		Projection GenerateProjection(double value, int years);
	}
}