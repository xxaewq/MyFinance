using Finance.Repository.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.Abstraction
{
    public interface ITypeRepository
    {
        Task<bool> CreateType(MstType type, CancellationToken token);
        Task<bool> DeleteType(Guid id, CancellationToken token);
        Task<List<MstType>> GetAllTypes(CancellationToken token);
        Task<MstType> GetTypeById(Guid id, CancellationToken token);
        Task<bool> UpdateType(MstType type, CancellationToken token);
    }
}
