using AutoMapper;
using Azure.Core.Serialization;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstUser;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Api.Controllers.Master;

[Route("api/master")]
[ApiController]
public class UserController(IMstUserRepository userRepository
        , IMapper mapper
        , IValidator<MstUserCreateModel> validatorCreate
        , IValidator<MstUserUpdateInforModel> validatorUpdateInfor) : ControllerBase
{
    private readonly IMstUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<MstUserCreateModel> _validatorCreate = validatorCreate;
    private readonly IValidator<MstUserUpdateInforModel> _validatorUpdateInfor = validatorUpdateInfor;

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

    [HttpGet("user/checkpassword")]
    public async Task<IActionResult> CheckPassword([FromBody] UserLoginModel user, CancellationToken token = default)
    {
        int result = await _userRepository.CheckPasswordAsync(user.UserName, user.Password, token);
        if (result == 0)
        {
            return BadRequest("Password is not match");
        }
        if (result == -2)
        {
            return BadRequest("User not found");
        }
        if (result == -1)
        {
            return BadRequest("User is not enabled");
        }
        return Ok();
    }


    [HttpPost("user")]
    public async Task<IActionResult> CreateUser([FromBody] MstUserCreateModel model, CancellationToken token = default)
    {
        var validationResult = await _validatorCreate.ValidateAsync(model, token);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }
        string createdBy = User.Identity?.Name ?? "Unknown";
        model.CreatedBy = createdBy;
        var user = _mapper.Map<MstUser>(model);

        bool isCreated = await _userRepository.CreateUserAsync(user, token);
        if (!isCreated)
        {
            return BadRequest("User could not be created");
        }
        var userResponseModel = _mapper.Map<MstUserResponseModel>(user);
        return Ok(userResponseModel);
    }
    [HttpPut("user/{id:guid}/updateinfor")]
    public async Task<IActionResult> UpdateUserInfor(Guid id, [FromBody] MstUserUpdateInforModel userModel, CancellationToken token = default)
    {
        var validationResult = await _validatorUpdateInfor.ValidateAsync(userModel, token);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }

        userModel.UpdateBy = User.Identity?.Name ?? "Unknown";
        userModel.Id = id;
        var user = _mapper.Map<MstUser>(userModel);
        bool isUpdated = await _userRepository.UpdateInforAsync(user, token);
        if (!isUpdated)
        {
            return BadRequest("User could not be updated");
        }
        return Ok();
    }

    [HttpPut("user/{id:guid}/updatepassword")]
    public async Task<IActionResult> UpdateUserPassword(Guid id, [FromBody] MstUserUpdatePasswordModel userModel, CancellationToken token = default)
    {
        userModel.Id = id;
        userModel.UpdateBy = User.Identity?.Name ?? "Unknown";
        var user = _mapper.Map<MstUser>(userModel);
        bool isUpdated = await _userRepository.UpdatePasswordAsync(user, token);
        if (!isUpdated)
        {
            return BadRequest("User password could not be updated");
        }
        return Ok();
    }

    [HttpDelete("user/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken token = default)
    {
        string deletedBy = User.Identity?.Name ?? "Unknown";
        bool isDeleted = await _userRepository.DeleteUserAsync(id, deletedBy, token);
        if (!isDeleted)
        {
            return BadRequest("User could not be deleted");
        }
        return Ok();
    }
}
