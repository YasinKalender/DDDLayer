using DDDLayer.Application.DTOs;
using DDDLayer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDDLayer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Bu end point token üretir
        /// </summary>
        /// <param name="loginDto"></param>
        /// <remarks>Örnek: https://localhost:7211/api/auth/CreateToken</remarks>
        /// <returns></returns>

        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Auth(LoginDto loginDto)
        {
            
            var result = await _authService.CreateTokenAsync(loginDto);

            return Ok(result);

        }


    }
}
