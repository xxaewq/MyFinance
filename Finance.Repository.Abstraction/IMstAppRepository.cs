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
        Task<List<MstApp>> GetAllAsync(CancellationToken token);
    }
}
