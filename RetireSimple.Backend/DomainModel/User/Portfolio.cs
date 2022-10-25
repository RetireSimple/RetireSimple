using RetireSimple.Backend.DomainModel.Data;

namespace RetireSimple.Backend.DomainModel.User {
    public class Portfolio {
        List<InvestmentBase> investments;
        List<InvestmentVehicleBase> investmentVehicles;
        List<ExpenseBase> expenses;
        List<InvestmentTransfer> transfers;

        public Portfolio() {
            investments = new List<InvestmentBase>();
            investmentVehicles = new List<InvestmentVehicleBase>();
            expenses = new List<ExpenseBase>();
            transfers = new List<InvestmentTransfer>();
        }

        public void generateFullAnalysis() { }
    }
}
