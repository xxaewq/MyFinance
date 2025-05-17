using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Shared.Models.MstApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.Tasks;

namespace Finance.Api.Controllers.Master
{
    [Route("api/master")]
    [ApiController]
    public class AppController(IMstAppRepository appRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMstAppRepository _appRepository = appRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Route("apps")]
        public async Task<IActionResult> GetAllApps(CancellationToken token)
        {
            var apps = await _appRepository.GetAllAsync(token);
            var appModels = _mapper.Map<List<MstAppResponseModel>>(apps);
            return Ok(appModels);
        }
        [HttpGet]
        [Route("app/{id:guid}")]
        public IActionResult GetAppById(Guid id)
        {
            // Simulate fetching data from a repository
            var app = "App" + id.ToString();
            if (app == null)
            {
                return NotFound("App could not be found");
            }
            return Ok(app);
        }

        [HttpPost]
        [Route("app")]
        public IActionResult CreateApp([FromBody] string app)
        {
            // Simulate creating an app
            if (string.IsNullOrEmpty(app))
            {
                return BadRequest("App name cannot be empty");
            }
            // Simulate saving the app to a repository
            return CreatedAtAction(nameof(GetAppById), new { id = Guid.NewGuid() }, app);
        }

        [HttpPut]
        [Route("app/{id:guid}")]
        public IActionResult UpdateApp(Guid id, [FromBody] string app)
        {
            // Simulate updating an app
            if (string.IsNullOrEmpty(app))
            {
                return BadRequest("App name cannot be empty");
            }
            // Simulate saving the updated app to a repository
            return NoContent();
        }

        [HttpDelete]
        [Route("app/{id:guid}")]
        public IActionResult DeleteApp(Guid id)
        {
            // Simulate deleting an app
            // Simulate removing the app from a repository
            return NoContent();
        }
    }
}
