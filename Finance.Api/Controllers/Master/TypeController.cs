using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstType;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Api.Controllers.Master
{
    [Route("api/master")]
    [ApiController]
    public class TypeController(IMstTypeRepository typeRepository
        , ILogger<TypeController> logger
        , IValidator<MstTypeCreateModel> validatorCreate
        , IValidator<MstTypeUpdateModel> validatorUpdate
        , IMapper mapper) : ControllerBase
    {
        private readonly IMstTypeRepository _typeRepository = typeRepository;
        private readonly ILogger<TypeController> _logger = logger;
        private readonly IValidator<MstTypeCreateModel> _validatorCreate = validatorCreate;
        private readonly IValidator<MstTypeUpdateModel> _validatorUpdate = validatorUpdate;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Route("types")]
        public async Task<IActionResult> GetAllTypes(CancellationToken token = default)
        {
            _logger.LogInformation("GetAllTypes called");
            var types = await _typeRepository.GetAllsAsync(token);
            var typeModels = _mapper.Map<List<MstTypeResponseModel>>(types);
            return Ok(typeModels);
        }

        [HttpGet("type/{id:guid}")]
        public async Task<IActionResult> GetTypeById(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("GetTypeById called with id: {id}", id);
            var type = await _typeRepository.GetByIdAsync(id, token);
            if (type == null)
            {
                return NotFound("Type could not be found");
            }
            var typeModel = _mapper.Map<MstTypeResponseModel>(type);
            return Ok(typeModel);
        }

        [HttpPost]
        [Route("type")]
        public async Task<IActionResult> CreateType([FromBody] MstTypeCreateModel mstType, CancellationToken token = default)
        {
            _logger.LogInformation("CreateType called");
            var validationResult = await _validatorCreate.ValidateAsync(mstType, token);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            string createdBy = User.Identity?.Name ?? "Unknown";
            mstType.CreatedBy = createdBy;
            var type = _mapper.Map<MstType>(mstType);

            bool isCreated = await _typeRepository.CreateTypeAsync(type, token);
            if (!isCreated)
            {
                return BadRequest("Type could not be created.");
            }

            var typeResponseModel = _mapper.Map<MstTypeResponseModel>(type);

            return Ok(typeResponseModel);
        }

        [HttpPut("type/{id:guid}")]
        public async Task<IActionResult> UpdateType(Guid id, [FromBody] MstTypeUpdateModel mstType, CancellationToken token = default)
        {
            _logger.LogInformation("UpdateType called with id: {id}", id);
            var validationResult = await _validatorUpdate.ValidateAsync(mstType, token);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            string updatedBy = User.Identity?.Name ?? "Unknown";
            mstType.UpdatedBy = updatedBy;
            mstType.Id = id;
            var type = _mapper.Map<MstType>(mstType);

            bool isUpdated = await _typeRepository.UpdateTypeAsync(type, token);
            if (!isUpdated)
            {
                return BadRequest("Type could not be updated.");
            }
            var typeResponseModel = _mapper.Map<MstTypeResponseModel>(type);
            return Ok(typeResponseModel);
        }
        [HttpDelete("type/{id:guid}")]
        public async Task<IActionResult> DeleteType(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("DeleteType called with id: {id}", id);
            string deleteBy = User.Identity?.Name ?? "Unknown";
            bool isDeleted = await _typeRepository.DeleteTypeAsync(id, deleteBy, token);
            if (!isDeleted)
            {
                return BadRequest("Type could not be deleted.");
            }
            return Ok();
        }
    }
}
