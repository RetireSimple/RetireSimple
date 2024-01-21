using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using RetireSimple.NewEngine.New_Engine.Users;

namespace RetireSimple.NewEngine.New_Engine {
	public class NewEngineMain {

		private User user;
		
		public NewEngineMain() {
			this.user = new User();
		
		}
	

		public Boolean HandleCreateUser(UserInfo info) {
			this.user.UpdateInfo(info);
			return true;
		}

		public UserInfo HandleReadUser() {
			var log = new StringBuilder();
			log.Append(this.user.GetInfo().ToString());

			System.IO.File.WriteAllText("..\\logs\\" + "log" + ".txt", log.ToString());
			return this.user.GetInfo();
		}

		public Boolean HandleUpdateUser(UserInfo info) {
			this.user.UpdateInfo(info);
			return true;
		}


		




	}
}
