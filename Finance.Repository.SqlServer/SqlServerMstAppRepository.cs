using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.SqlServer
{
    public class SqlServerMstAppRepository(SqlServerDatabaseHelper helper) : IMstAppRepository
    {
        private readonly SqlServerDatabaseHelper _helper = helper;

        public async Task<List<MstApp>> GetAllAsync(CancellationToken token)
        {
            string sql = "SELECT Id, TypeApp, NameApp, Description FROM MstApp";
            var result = await _helper.ExecuteQuery(sql, reader => new MstApp
            {
                Id = reader.GetGuid(0),
                TypeApp = reader.GetString(1),
                TypeName = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3)
            }, token);

            return result;
        }
    }
}
