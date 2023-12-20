using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.User {
	public class UserInfo {


		public readonly DateOnly birthday;

		public int retirementAge;

		public long retirementGoal;


		public UserInfo(DateOnly birthday, int retirementAge, long retirementGoal ) {
			this.birthday = birthday;
			this.retirementAge = retirementAge;
			this.retirementGoal = retirementGoal;
		}

	}
}
