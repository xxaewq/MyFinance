using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Api.Controllers.Master
{
    [Route("api/master")]
    [ApiController]
    public class UserController(IMstUserRepository userRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMstUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers(CancellationToken token = default)
        {
            var users = await _userRepository.GetAllUsersAsync(token);
            var userModels = _mapper.Map<List<MstUserResponseModel>>(users);
            return Ok(userModels);
        }
        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken token = default)
        {
            var user = await _userRepository.GetByIdAsync(id, token);
            if (user == null)
            {
                return NotFound("User could not be found");
            }
            var userModel = _mapper.Map<MstUserResponseModel>(user);
            return Ok(userModel);
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] MstUserCreateModel model, CancellationToken token = default)
        {

            var user = _mapper.Map<MstUser>(model);
            bool isCreated = await _userRepository.CreateUserAsync(user, token);
            if (!isCreated)
            {
                return BadRequest("User could not be created");
            }
            var userResponseModel = _mapper.Map<MstUserResponseModel>(user);
            return Ok(userResponseModel);
        }
        [HttpPut("user/{id:guid}")]
        public IActionResult UpdateUser(Guid id, [FromBody] string userName)
        {
            // This is a placeholder for the actual implementation.
            // You would typically update a user in a database or another service.
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("User name cannot be empty.");
            }
            return Ok($"User with ID: {id} updated to {userName}");
        }
        [HttpDelete("user/{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            // This is a placeholder for the actual implementation.
            // You would typically delete a user from a database or another service.
            return Ok($"User with ID: {id} deleted");
        }
    }
}
