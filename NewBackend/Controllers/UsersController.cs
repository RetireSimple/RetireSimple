using Microsoft.AspNetCore.Mvc;

using NewBackend.Models;
using NewBackend.Services;

namespace UserstoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
	private readonly UsersService _UsersService;

	public UsersController(UsersService UsersService) =>
		_UsersService = UsersService;

	[HttpGet]
	public async Task<List<Users>> Get() =>
		await _UsersService.GetAsync();

	[HttpPost]
	public async Task<IActionResult> Post(Users newUsers) {
		await _UsersService.CreateAsync(newUsers);

		return CreatedAtAction(nameof(Get), new { id = newUsers.Id }, newUsers);
	}

	[HttpPut]
	public async Task<IActionResult> Update(string id, Users updatedUsers) {
		var Users = await _UsersService.GetAsync(id);

		if (Users is null) {
			return NotFound();
		}

		updatedUsers.Id = Users.Id;

		await _UsersService.UpdateAsync(id, updatedUsers);

		return NoContent();
	}

	
}