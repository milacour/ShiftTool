using Microsoft.AspNetCore.Mvc;
using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;
using System.Threading.Tasks;

namespace ShiftTool.Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Registrer en ny bruger
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(UserDTO userDto)
        {
            var user = ConvertFromDTO(userDto);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.CreateUserAsync(user);
            return Ok("Bruger registreret succesfuldt");
        }

        // Log ind en bruger
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserDTO userDto)
        {
            var user = await _userRepository.GetCurrentUserAsync(userDto.Email);
            if (user != null && user.Password == userDto.Password)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserId", user.UserId.ToString());
                return Ok(new { Message = "Login succesfuldt" });
            }
            return Unauthorized("Ugyldige legitimationsoplysninger");
        }

        // Log ud en bruger
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(bool permanent = false)
        {
            try
            {
                if (!permanent)
                {
                    // Hent brugerens id fra sessionen
                    var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

                    if (!string.IsNullOrEmpty(userId))
                    {
                        // Fjern UserId fra sessionen
                        _httpContextAccessor.HttpContext.Session.Remove("UserId");
                    }
                }
                else
                {
                    // Permanent logout, fjern UserId fra sessionen
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        _httpContextAccessor.HttpContext.Session.Remove("UserId");
                    }
                }

                return Ok(new { Message = "Logget ud succesfuldt" });
            }
            catch (Exception ex)
            {
                // Håndter eventuelle fejl her
                return StatusCode(500, new { Message = "Der opstod en fejl under logud." });
            }
        }

        // Opret en bruger
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync(UserDTO userDto)
        {
            var user = ConvertFromDTO(userDto);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.CreateUserAsync(user);
            return Ok("Bruger oprettet succesfuldt");
        }

        // Hent alle brugere
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        // Hent en bruger ved e-mail
        [HttpGet("get-user/{email}")]
        public async Task<IActionResult> GetCurrentUserAsync(string email)
        {
            var user = await _userRepository.GetCurrentUserAsync(email);
            return user != null ? Ok(user) : NotFound("Bruger blev ikke fundet");
        }

        // Opdater en bruger
        [HttpPut("update-user/{email}")]
        public async Task<IActionResult> UpdateUserAsync(string email, UserDTO userDto)
        {
            var existingUser = await _userRepository.GetCurrentUserAsync(email);
            if (existingUser == null) return NotFound("Bruger blev ikke fundet");

            UpdateUserFromDTO(existingUser, userDto);
            await _userRepository.UpdateUserAsync(existingUser);
            return Ok("Bruger opdateret succesfuldt");
        }

        // Slet en bruger
        [HttpDelete("delete-user/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _userRepository.DeleteUserAsync(email);
            return Ok("Bruger slettet succesfuldt");
        }

        // Konverter fra UserDTO til User model
        private User ConvertFromDTO(UserDTO userDto)
        {
            return new User
            {
                Email = userDto.Email,
                Password = userDto.Password,
                FullName = userDto.FullName,
                PhoneNumber = userDto.PhoneNumber,
                IsCoordinator = userDto.IsCoordinator,
                Experience = userDto.Experience,
                Skills = userDto.Skills
                // Bemærk: CreatedAt sættes typisk ved oprettelsen
            };
        }

        // Opdater eksisterende User model fra UserDTO
        private void UpdateUserFromDTO(User user, UserDTO userDto)
        {
            user.FullName = userDto.FullName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.IsCoordinator = userDto.IsCoordinator;
            user.Experience = userDto.Experience;
            user.Skills = userDto.Skills;
            // Bemærk: Password og CreatedAt opdateres ikke her
        }
    }
}
