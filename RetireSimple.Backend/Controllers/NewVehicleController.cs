using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.Base;
using RetireSimple.Engine.New_Engine;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles._401k;
using RetireSimple.NewEngine.New_Engine.Financials.InvestmentVehicles.InvestmentVehicleInfos;
using RetireSimple.NewEngine.New_Engine.TaxModels;
using RetireSimple.NewEngine.New_Engine.User;

using System.Text.Json;


namespace RetireSimple.Backend.Controllers {

	[Route("api/[controller]")]
	[ApiController]

	public class NewVehicleController : ControllerBase {

		[HttpGet]
		public ActionResult<String> TestingAPI() => Ok("API EndPoint is Working");

		/*
        [HttpPost]

        public ActionResult createNewVehicle([FromBody] JsonDocument requestBody){
            var body = requestBody.Deserialize<OptionsDict>();
			if (body == null) {
				return BadRequest();
			} else {

				new

				User user = new User()
			}
           
        

        }


    }
		*/

		[HttpGet]
		[Route("Test")]

		public ActionResult<Projection> getTestData() {

			User user = new User(new UserInfo(30, 65, 1000000));

			_401kInfo info = new _401kInfo(.1, 40000, 0, .07, .5, .06);

			_401k _401K = new _401k(new NullTax(), 1000, 1, info);

			user.AddInvestmentVehicle(_401K);

			return Ok(user.GenerateProjections());



		}


	}



}