using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using RetireSimple.NewEngine.New_Engine.Users;

namespace RetireSimple.NewEngine.New_Engine {
	public class NewEngineMain {



		private User user;






		public Boolean handleCreateUser(UserInfo info) {
			this.user = new User(info);
			return true;
		}

		public UserInfo handleReadUser() {
			return user.GetInfo();
		}

		public Boolean handleUpdateUser(UserInfo info) {
			this.user.UpdateInfo(info);
			return true;
		}


		




	}
}
