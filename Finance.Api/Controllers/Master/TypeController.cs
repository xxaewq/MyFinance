using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstType;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Api.Controllers.Master
{
    [Route("api/master/[controller]")]
    [ApiController]
    public class TypeController(ITypeRepository typeRepository
        , ILogger<TypeController> logger
        , IValidator<MstTypeCreateModel> validatorCreate
        , IValidator<MstTypeUpdateModel> validatorUpdate
        , IMapper mapper) : ControllerBase
    {
        private readonly ITypeRepository _typeRepository = typeRepository;
        private readonly ILogger<TypeController> _logger = logger;
        private readonly IValidator<MstTypeCreateModel> _validatorCreate = validatorCreate;
        private readonly IValidator<MstTypeUpdateModel> _validatorUpdate = validatorUpdate;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAllTypes(CancellationToken token = default)
        {
            _logger.LogInformation("GetAllTypes called");
            var types = await _typeRepository.GetAllTypes(token);
            var typeModels = _mapper.Map<List<MstTypeResponseModel>>(types);
            return Ok(typeModels);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTypeById(Guid id, CancellationToken token = default)
        {
            var type = await _typeRepository.GetTypeById(id, token);
            if (type == null)
            {
                return NotFound("Type could not be found");
            }
            var typeModel = _mapper.Map<MstTypeResponseModel>(type);
            return Ok(typeModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateType([FromBody] MstTypeCreateModel mstType, CancellationToken token = default)
        {
            var validationResult = await _validatorCreate.ValidateAsync(mstType, token);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var type = _mapper.Map<MstType>(mstType);

            bool isCreated = await _typeRepository.CreateType(type, token);
            if (!isCreated)
            {
                return BadRequest("Type could not be created.");
            }

            var typeResponseModel = _mapper.Map<MstTypeResponseModel>(type);

            return Ok(typeResponseModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateType(Guid id, [FromBody] MstTypeUpdateModel mstType, CancellationToken token = default)
        {
            var validationResult = await _validatorUpdate.ValidateAsync(mstType, token);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }
            var type = _mapper.Map<MstType>(mstType);
            type.Id = id;

            bool isUpdated = await _typeRepository.UpdateType(type, token);
            if (!isUpdated)
            {
                return BadRequest("Type could not be updated.");
            }
            var typeResponseModel = _mapper.Map<MstTypeResponseModel>(type);
            return Ok(typeResponseModel);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteType(Guid id, CancellationToken token = default)
        {
            bool isDeleted = await _typeRepository.DeleteType(id, token);
            if (!isDeleted)
            {
                return BadRequest("Type could not be deleted.");
            }
            return Ok(id);
        }
    }
}
