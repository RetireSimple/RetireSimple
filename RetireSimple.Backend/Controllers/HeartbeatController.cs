using Microsoft.AspNetCore.Mvc;

namespace RetireSimple.Backend.Controllers {

	/// <summary>
	/// Simple controller to check if the server is up and running.
	/// Used by the electron wrapper to redirect to static files if the server is ready
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class HeartbeatController : ControllerBase {
		[HttpGet]
		public IActionResult Get() {
			return Ok();
		}
	}
}