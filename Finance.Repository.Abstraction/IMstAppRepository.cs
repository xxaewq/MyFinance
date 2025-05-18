using Finance.Repository.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction
{
    public interface IMstAppRepository
    {
        Task<bool> CreateAsync(MstApp app, CancellationToken token);
        Task<bool> DeleteAsync(Guid id, string deletedBy, CancellationToken token);
        Task<List<MstApp>> GetAllAsync(CancellationToken token);
        Task<MstApp?> GetByIdAsync(Guid id, CancellationToken token);
        Task<bool> UpdateAsync(MstApp app, CancellationToken token);
    }
}
