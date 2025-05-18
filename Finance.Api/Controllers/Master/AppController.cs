using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstApp;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Api.Controllers.Master;

[Route("api/master")]
[ApiController]
public class AppController(IMstAppRepository appRepository
    , IMapper mapper
    , IValidator<MstAppCreateModel> validatorCreate
    , IValidator<MstAppUpdateModel> validatorUpdate) : ControllerBase
{
    private readonly IMstAppRepository _appRepository = appRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<MstAppCreateModel> _validatorCreate = validatorCreate;
    private readonly IValidator<MstAppUpdateModel> _validatorUpdate = validatorUpdate;

    [HttpGet]
    [Route("apps")]
    public async Task<IActionResult> GetAllApps(CancellationToken token = default)
    {
        var apps = await _appRepository.GetAllAsync(token);
        var appModels = _mapper.Map<List<MstAppResponseModel>>(apps);
        return Ok(appModels);
    }
    [HttpGet]
    [Route("app/{id:guid}")]
    public async Task<IActionResult> GetAppById(Guid id, CancellationToken token = default)
    {
        var app = await _appRepository.GetByIdAsync(id, token);
        if (app == null)
        {
            return NotFound();
        }
        var appModel = _mapper.Map<MstAppResponseModel>(app);
        return Ok(appModel);
    }

    [HttpPost]
    [Route("app")]
    public async Task<IActionResult> CreateApp([FromBody] MstAppCreateModel mstApp, CancellationToken token = default)
    {
        var validationResult = await _validatorCreate.ValidateAsync(mstApp, token);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }
        string createdBy = User.Identity?.Name ?? "Unknown";
        mstApp.CreateBy = createdBy;
        var app = _mapper.Map<MstApp>(mstApp);
        bool isCreated = await _appRepository.CreateAsync(app, token);
        if (!isCreated)
        {
            return BadRequest("App could not be created.");
        }
        var appModel = _mapper.Map<MstAppResponseModel>(app);
        return Ok(appModel);
    }

    [HttpPut]
    [Route("app/{id:guid}")]
    public async Task<IActionResult> UpdateApp(Guid id, [FromBody] MstAppUpdateModel mstApp, CancellationToken token = default)
    {
        var validationResult = await _validatorUpdate.ValidateAsync(mstApp, token);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }
        string updatedBy = User.Identity?.Name ?? "Unknown";
        mstApp.UpdateBy = updatedBy;
        mstApp.Id = id;
        var app = _mapper.Map<MstApp>(mstApp);
        bool isUpdated = await _appRepository.UpdateAsync(app, token);
        if (!isUpdated)
        {
            return BadRequest("App could not be updated.");
        }
        var appModel = _mapper.Map<MstAppResponseModel>(app);
        return Ok(appModel);
    }

    [HttpDelete]
    [Route("app/{id:guid}")]
    public async Task<IActionResult> DeleteApp(Guid id, CancellationToken token)
    {
        string deletedBy = User.Identity?.Name ?? "Unknown";
        bool isDeleted = await _appRepository.DeleteAsync(id, deletedBy, token);
        if (!isDeleted)
        {
            return BadRequest("App could not be deleted.");
        }
        return Ok();
    }
}
