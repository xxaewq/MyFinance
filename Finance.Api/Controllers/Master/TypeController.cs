using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers.Master
{
    [Route("api/master/[controller]")]
    [ApiController]
    public class TypeController(ITypeRepository typeRepository
        , ILogger<TypeController> logger
        , IMapper mapper) : ControllerBase
    {
        private readonly ITypeRepository _typeRepository = typeRepository;
        private readonly ILogger<TypeController> _logger = logger;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetAllTypes()
        {
            _logger.LogInformation("GetAllTypes called");
            var types = await _typeRepository.GetAllTypes();
            var typeModels = _mapper.Map<List<MstTypeResponseModel>>(types);
            return Ok(typeModels);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTypeById(Guid id)
        {
            var type = await _typeRepository.GetTypeById(id);
            if (type == null)
            {
                return NotFound();
            }
            var typeModel = _mapper.Map<MstTypeResponseModel>(type);
            return Ok(typeModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateType([FromBody]MstTypeCreateModel mstType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var type = _mapper.Map<MstType>(mstType);

            await _typeRepository.CreateType(type);

            var typeResponseModel = _mapper.Map<MstTypeResponseModel>(type);

            return Ok(typeResponseModel);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateType(Guid id)
        {
            return Ok();
        }
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteType(Guid id)
        {
            return Ok();
        }
    }
}
