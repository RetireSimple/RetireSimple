using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;

namespace RetireSimple.NewEngine.New_Engine.GrowthModels {
	public interface IGrowthModel {
		Projection GenerateProjection(double value, int years, InvestmentVehicleInfo info);

	}
}