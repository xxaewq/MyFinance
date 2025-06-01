using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.UserBalance;

namespace Finance.Repository.Abstraction
{
    public interface IUserBalanceRepository
    {
        Task<bool> CreateUserBalanceAsync(UserBalance userBalance, CancellationToken token);
        Task<List<UserBalanceModel>> GetByUserIdAsync(Guid userId, CancellationToken token);
    }
}
