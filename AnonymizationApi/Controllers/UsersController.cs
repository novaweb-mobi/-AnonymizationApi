using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AnonymizationApi.Data;
using AnonymizationApi.Models;
using AnonymizationApi.Services;

namespace AnonymizationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("anonymize")]
        public async Task<IActionResult> AnonymizeUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                await _userService.RandomizeAndSaveUserAsync(user);
                var anonymizedUser = await _userService.GetAnonymizedUserAsync(user);
                if (anonymizedUser == null)
                {
                    return NotFound("Anonymized user not found.");
                }

                return Ok(new
        {
            anonymizedUser.Name,
            anonymizedUser.AnonymizedCpf,
            anonymizedUser.AnonymizedDateOfBirth,
            anonymizedUser.Gender
        });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
