using Finance.Repository.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction
{
    public interface IMstTypeRepository
    {
        Task<bool> CreateTypeAsync(MstType type, CancellationToken token);
        Task<bool> DeleteTypeAsync(Guid id, string deleteBy, CancellationToken token);
        Task<List<MstType>> GetAllsAsync(CancellationToken token);
        Task<MstType?> GetByIdAsync(Guid id, CancellationToken token);
        Task<bool> UpdateTypeAsync(MstType type, CancellationToken token);
    }
}
