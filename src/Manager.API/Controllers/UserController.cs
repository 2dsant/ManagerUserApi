using Microsoft.AspNetCore.Authorization;
using Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Exceptions;
using System.Threading.Tasks;
using Manager.API.ViewModels;
using Manager.API.Utilities;
using Manager.Services.DTO;
using AutoMapper;
using System;

namespace Manager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/api/v1/users/create")]
        public async Task<IActionResult> Create([FromBody]CreateUserViewModel userViewModel)
        {
            try
            {
                var userDto = _mapper.Map<UserDTO>(userViewModel);
                var result = await _userService.Create(userDto);
                return Ok(new ResultViewModel{
                    Message = "Usuário criado com sucesso.",
                    Success = true,
                    Data = result
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }

        [HttpPut]
        [Route("/api/v1/users/update")]
        public async Task<IActionResult> Update([FromBody]UpdateUserViewModel userViewModel)
        {
            try
            {
                var userDto = _mapper.Map<UserDTO>(userViewModel);
                var result = await _userService.Update(userDto);
                return Ok(new ResultViewModel{
                    Message = "Usuário atualizado com sucesso.",
                    Success = true,
                    Data = result
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }

        [HttpDelete]
        [Route("/api/v1/users/remove/{id}")]
        public async Task<IActionResult> Remove(long id)
        {
            try
            {
                await _userService.Remove(id);
                return Ok(new ResultViewModel{
                    Message = "Usuário deletado com sucesso.",
                    Success = true,
                    Data = null
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }

        [HttpGet]
        [Route("/api/v1/users/get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var user = await _userService.Get(id);

                if(user == null)
                {
                    return NotFound(new ResultViewModel{
                        Message = "Nenhum usuário foi encontrado.",
                        Success = false,
                        Data = null
                    });
                }

                return Ok(new ResultViewModel{
                    Message = "Usuário encontrado.",
                    Success = true,
                    Data = user
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }

        [HttpGet]
        [Route("/api/v1/users/get-all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var allUsers = await _userService.Get();

                if(allUsers.Count == 0)
                {
                    return NotFound(new ResultViewModel{
                        Message = "Nenhum usuário foi encontrado.",
                        Success = false,
                        Data = null
                    });
                }

                return Ok(new ResultViewModel{
                    Message = "Usuário encontrado.",
                    Success = true,
                    Data = allUsers
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }

        [HttpGet]
        [Route("/api/v1/users/search-by-email")]
        public async Task<IActionResult> SearchByEmail([FromQuery]string email)
        {
            try
            {
                var result = await _userService.SearchByEmail(email);
                if(result.Count != 0)
                {
                    return Ok(new ResultViewModel{
                        Message = "Usuário encontrado.",
                        Success = true,
                        Data = result
                    });
                }
                else 
                {
                    return NotFound(new ResultViewModel{
                        Message = "Usuário não encontrado.",
                        Success = false,
                        Data = result
                    });
                }
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }

        [HttpGet]
        [Route("/api/v1/users/search-by-name")]
        public async Task<IActionResult> SearchByName([FromQuery]string name)
        {
            try
            {
                var result = await _userService.SearchByName(name);
                if(result.Count != 0)
                {
                    return Ok(new ResultViewModel{
                        Message = "Usuário encontrado.",
                        Success = true,
                        Data = result
                    });
                }
                else 
                {
                    return NotFound(new ResultViewModel{
                        Message = "Usuário não encontrado.",
                        Success = false,
                        Data = result
                    });
                }
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }   
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }  
        }
    }
}