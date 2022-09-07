using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.API.Token;
using Manager.API.Utilities;
using Manager.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Manager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        public IActionResult Login([FromBody]LoginViewModel login)
        {
            try
            {
                var tokenLogin = _configuration["Jwt:Login"];
                var tokenPassword = _configuration["Jwt:Password"];

                if (login.Login == tokenLogin && login.Password == tokenPassword)
                    return Ok(new ResultViewModel
                    {
                        Message = "Usuário autenticado com sucesso!",
                        Success = true,
                        Data = new
                        {
                            Token = _tokenService.GenerateToken(),
                            TokenExpires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]))
                        }
                    });
                else
                    return StatusCode(401, Responses.UnauthorizedErrorMessage());
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}