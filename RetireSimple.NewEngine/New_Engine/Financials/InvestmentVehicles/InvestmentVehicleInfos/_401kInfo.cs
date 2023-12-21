namespace RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos {
	public class _401kInfo : InvestmentVehicleInfo{

		//What percent of your salary do you plan on contributing yearly
		public double contributions;

		//How much money do you make a year
		public double salary;

		//How much do you expect for your salary to increase in a given year
		public double salaryIncrease;

		//What is your expected rate of return
		public double rate;

		//How much does your employer match
		public double employerMatch;

		//Up to what percent do they match until 
		public double employerMatchCap;


		public _401kInfo(double contributions, double salary, double salaryIncrease, double rate, double employerMatch, double employerMatchCap) {
			this.contributions = contributions;
			this.salary = salary;
			this.salaryIncrease = salaryIncrease;
			this.rate = rate;
			this.employerMatch = employerMatch;
			this.employerMatchCap = employerMatchCap;
		}

	}
}