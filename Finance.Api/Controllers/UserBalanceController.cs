using AutoMapper;
using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.UserBalance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBalanceController(IUserBalanceRepository userBalanceRepository
        , IMapper mapper) : ControllerBase
    {
        private readonly IUserBalanceRepository _userBalanceRepository = userBalanceRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetByUserIdAsync(Guid userId, CancellationToken token =default)
        {
            var userBalances = await _userBalanceRepository.GetByUserIdAsync(userId, token);
            return Ok(userBalances);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserBalanceCreateModel model, CancellationToken token = default)
        {

            var userBalance = _mapper.Map<UserBalance>(model);
            userBalance.CreateBy = User.Identity?.Name ?? "Unknown";

            bool isCreated = await _userBalanceRepository.CreateUserBalanceAsync(userBalance, token);
            if(!isCreated)
            {
                return BadRequest("Failed to create user balance");
            }
            return Ok();
        }
    }
}
