using Finance.Repository.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction
{
    public interface IMstUserRepository
    {
        Task<bool> CreateUserAsync(MstUser user, CancellationToken token);
        Task<List<MstUser>> GetAllUsersAsync(CancellationToken token);
        Task<MstUser?> GetByIdAsync(Guid id, CancellationToken token);
    }
}
