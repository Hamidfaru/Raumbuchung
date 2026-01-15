using Microsoft.AspNetCore.Mvc;
using Raumbuchung.API.DTOs.Account;
using Raumbuchung.API.Services.Account;

namespace Raumbuchung.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterDto registerDto)
        {
            try
            {
                var benutzer = await _authService.RegistrierenAsync(registerDto);
                if (benutzer == null)
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Benutzer mit dieser Email oder diesem Benutzernamen existiert bereits"
                    });

                var userDto = MapToUserDto(benutzer);

                return Ok(new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "Registrierung erfolgreich",
                    Data = userDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler bei der Registrierung");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Interner Serverfehler"
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login(LoginDto loginDto)
        {
            try
            {
                var benutzer = await _authService.LoginAsync(loginDto.LoginName, loginDto.Passwort);
                if (benutzer == null)
                    return Unauthorized(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Ungültige Anmeldedaten"
                    });

                var userDto = MapToUserDto(benutzer);

                return Ok(new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "Login erfolgreich",
                    Data = userDto
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Login");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Interner Serverfehler"
                });
            }
        }

        [HttpGet("check-username/{benutzername}")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckBenutzername(string benutzername)
        {
            try
            {
                var existiert = await _authService.GetBenutzerByBenutzernameAsync(benutzername);
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = existiert == null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Prüfen des Benutzernamens");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Interner Serverfehler"
                });
            }
        }

        [HttpGet("check-email/{email}")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmail(string email)
        {
            try
            {
                var existiert = await _authService.GetBenutzerByEmailAsync(email);
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = existiert == null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Prüfen der Email");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Interner Serverfehler"
                });
            }
        }

        private UserDto MapToUserDto(Raumbuchung.API.Models.Account.Benutzer benutzer)
        {
            return new UserDto
            {
                BenutzerId = benutzer.BenutzerId,
                Benutzername = benutzer.Benutzername,
                Email = benutzer.Email,
                Telefonnummer = benutzer.Telefonnummer,
                VollerName = benutzer.VollerName,
                Abteilung = benutzer.Abteilung,
                IstAdmin = benutzer.IstAdmin,
                IstAktiv = benutzer.IstAktiv,
                ErstellungsDatum = benutzer.ErstellungsDatum,
                LetzterLogin = benutzer.LetzterLogin
            };
        }
    }
}