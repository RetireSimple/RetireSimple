using Microsoft.AspNetCore.Mvc;

using RetireSimple.Engine.Api;
using RetireSimple.Engine.Data;
using RetireSimple.Engine.Data.InvestmentVehicle;

using System.Text.Json;

namespace RetireSimple.Backend.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class VehicleController : ControllerBase {
		private readonly InvestmentVehicleApi _vehicleApi;

		public VehicleController(EngineDbContext context) {
			_vehicleApi = new InvestmentVehicleApi(context);
		}

		[HttpGet]
		public ActionResult<List<InvestmentVehicleBase>> GetVehicles() => Ok(_vehicleApi.GetInvestmentVehicles());


		[HttpGet]
		[Route("{id}")]
		public ActionResult<InvestmentVehicleBase> GetVehicle(int id) {
			try {
				var vehicle = _vehicleApi.GetInvestmentVehicle(id);
				return Ok(vehicle);
			}
			catch (ArgumentException) {
				return NotFound("Vehicle not found");
			}
		}

		//TODO Not Finished
		[HttpPost]
		[Route("Add")]
		public ActionResult AddVehicle([FromBody] JsonDocument requestBody) {
			var body = requestBody.Deserialize<OptionsDict>();
			if (body == null) {
				return BadRequest();
			}

			//Must have a vehicleType defined, we check if type is valid in API call
			if (!body.ContainsKey("vehicleType")) {
				return BadRequest("vehicleType not defined");
			}

			try {
				var type = body["vehicleType"];
				body.Remove("vehicleType");

				// var id = _vehicleApi.Add(type, body);

				//Check if we got analysis parameters
				if (body.Keys.Any(k => k.StartsWith("analysis_"))) {
					var analysisOptions =
						body.Where(kvp => kvp.Key.StartsWith("analysis_"))
							.ToDictionary(kvp => kvp.Key.Remove(0, 9), kvp => kvp.Value);

					// _vehicleApi.SetAnalysisOptions(id, analysisOptions);
				}

				// return Ok(id);
				return Ok();
			}
			catch (ArgumentException) {
				return BadRequest("Invalid vehicle type");
			}
		}

		[HttpDelete]
		[Route("Delete/{id}")]
		public ActionResult DeleteVehicle(int id) {
			try {
				_vehicleApi.Remove(id);
				return Ok();
			}
			catch (ArgumentException) {
				return NotFound("Vehicle not found");
			}
		}

		[HttpDelete]
		[Route("InvestmentDelete/{vehicleId}")]
		public ActionResult DeleteInvestmentFromVehicle(int vehicleId, [FromQuery] int investmentId) {
			try {
				_vehicleApi.RemoveInvestmentFromVehicle(vehicleId, investmentId);
				return Ok();
			}
			catch (ArgumentException) {
				return NotFound("Vehicle or investment not found");
			}
		}

		[HttpPost]
		[Route("InvestmentAdd/{vehicleId}")]
		public ActionResult AddInvestmentToVehicle(int vehicleId, [FromQuery] int investmentId) {
			try {
				_vehicleApi.AddInvestmentToVehicle(vehicleId, investmentId);
				return Ok();
			}
			catch (ArgumentException) {
				return NotFound("Vehicle or investment not found");
			}
		}

		[HttpPost]
		[Route("Update/{id}")]
		public ActionResult UpdateVehicle(int id, [FromBody] JsonDocument requestBody) {
			var body = requestBody.Deserialize<OptionsDict>();
			if (body == null) {
				return BadRequest();
			}

			try {
				// _vehicleApi.Update(id, body);
				if (body.Keys.Any(k => k.StartsWith("analysis_"))) {
					var analysisOptions =
						body.Where(kvp => kvp.Key.StartsWith("analysis_"))
							.ToDictionary(kvp => kvp.Key.Remove(0, 9), kvp => kvp.Value);

					// _vehicleApi.UpdateAnalysisOptions(id, analysisOptions);
				}

				return Ok();
			}
			catch (ArgumentException) {
				return NotFound("Vehicle not found");
			}
		}
	}
}