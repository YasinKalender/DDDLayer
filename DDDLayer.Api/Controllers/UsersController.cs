using DDDLayer.Application.DTOs;
using DDDLayer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDDLayer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Bu end point kullanıcı ekler
        /// </summary>
        /// <param name="createUserDto">Id</param>
        /// <returns></returns>
        /// <response code="404">Sayfa bulunamadı</response>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Users(CreateUserDto createUserDto)
        {
            return Ok(await _userService.CreateUserAsync(createUserDto));
        }

        /// <summary>
        /// Bu end point kullanıcı verir.
        /// </summary>
        /// <returns></returns>



        [Authorize]
        [HttpGet]
      
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }
    }
}
