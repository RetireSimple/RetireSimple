using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RetireSimple.NewEngine.New_Engine.User {
	public class UserInfo {


		public readonly int age;

		public int retirementAge;

		public long retirementGoal;


		public UserInfo(int age, int retirementAge, long retirementGoal ) {
			this.age = age;
			this.retirementAge = retirementAge;
			this.retirementGoal = retirementGoal;
		}

	}
}
