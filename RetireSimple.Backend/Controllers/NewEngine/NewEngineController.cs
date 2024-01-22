using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Users;

using System;
using System.Text;
using System.Text.Json;

using static System.Net.Mime.MediaTypeNames;

namespace RetireSimple.Backend.Controllers.NewEngine {

	[Route("api/[controller]")]
	[ApiController]

	public class NewEngineController : ControllerBase {

		private readonly NewEngineMain newEngineMain;

		public NewEngineController(NewEngineMain newEngineMain) {
			this.newEngineMain = newEngineMain;
		}

		[HttpGet]
		[Route("User")]
		public ActionResult<UserInfo> GetUser() {
			return Ok(this.newEngineMain.HandleReadUser());
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
				//Console.WriteLine(info);

				var log = new StringBuilder();
				log.Append(info.ToString());

				System.IO.File.WriteAllText("..\\logs\\" + "log" + ".txt", log.ToString());
				return Ok(this.newEngineMain.HandleCreateUser(info));
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

				return Ok(this.newEngineMain.HandleUpdateUser(info));
			} catch (ArgumentException) {
				return BadRequest();
			}

		}














	}


}

