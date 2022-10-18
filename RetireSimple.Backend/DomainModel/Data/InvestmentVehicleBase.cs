using RetireSimple.Backend.DomainModel.Data;

namespace RetireSimple.Backend.DomainModel.Data {
    public abstract class InvestmentVehicleBase {
        protected List<InvestmentBase> investments;

        public InvestmentVehicleBase() {
            investments = new List<InvestmentBase>();
        }

        public abstract void generateAggregateAnalysis();
    }
}