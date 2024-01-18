using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Users;

using System.Text.Json;


namespace RetireSimple.Backend.Controllers.NewEngine {

	[Route("api/[controller]")]
	[ApiController]

	public class NewEngineController : ControllerBase {

		private NewEngineMain newEngineMain;

		public NewEngineController() {
			this.newEngineMain = new NewEngineMain();
		}

		[HttpGet]
		[Route("User")]
		public ActionResult<UserInfo> GetUser() {
			return Ok(this.newEngineMain.handleReadUser());
		}


		[HttpPost]
		[Route("NewUser")]
		public ActionResult<Boolean> CreateUser([FromBody] JsonDocument requestBody) {

			var body = requestBody.Deserialize<OptionsDict>();

			if (body == null) {
				return BadRequest();
			}

			//TODO add in the error checking

			try {

				UserInfo info = new UserInfo(Convert.ToInt16(body["Age"]), Convert.ToInt16(body["RetirementAge"]),Convert.ToInt32(body["RetirementGoal"]), UserInfo.StringToStatus(body["UserTaxStatus"]));

				return Ok(this.newEngineMain.handleCreateUser(info));
			}
			catch (ArgumentException) {
				return BadRequest();
			}

		}

		[HttpPost]
		[Route("UpdateUser")]
		public ActionResult<Boolean> UpdateUser([FromBody] JsonDocument requestBody) {
			var body = requestBody.Deserialize<OptionsDict>();

			if(body == null) {
				return BadRequest();
			}

			//TODO add in the error checking

			try {

				UserInfo info = new UserInfo(Convert.ToInt16(body["Age"]), Convert.ToInt16(body["RetirementAge"]), Convert.ToInt32(body["RetirementGoal"]), UserInfo.StringToStatus(body["UserTaxStatus"]));

				return Ok(this.newEngineMain.handleUpdateUser(info));
			} catch (ArgumentException) {
				return BadRequest();
			}

		}














	}


}

