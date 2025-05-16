using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.SqlServer
{
    public class SqlServerTypeRepository : ITypeRepository
    {
        private readonly SqlServerDatabaseHelper _helper;
        public SqlServerTypeRepository(SqlServerDatabaseHelper helper)
        {
            _helper = helper;
        }

        public Task CreateType(MstType type)
        {
            string sql = "INSERT INTO MstType (Id, TypeName, Description) VALUES (@Id, @TypeName, @Description)";
            var parameters = new SqlParameter[]
            {
                new("@Id", type.Id),
                new("@TypeName", type.TypeName),
                new("@Description", type.Description)
            };
            return _helper.ExecuteNonQuery(sql, parameters);
        }

        public async Task<List<MstType>> GetAllTypes()
        {
            string sql = "SELECT * FROM MstType";
            var types = await _helper.ExecuteQuery(sql, reader => new MstType
            {
                Id = reader.GetGuid(0),
                TypeName = reader.GetString(1),
                Description = reader.GetString(2)
            });
            return types;
        }

        public async Task<MstType> GetTypeById(Guid id)
        {
            string sql = "SELECT * FROM MstType WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new("@Id", id)
            };
            var type = await _helper.ExecuteQuery(sql, reader => new MstType
            {
                Id = reader.GetGuid(0),
                TypeName = reader.GetString(1),
                Description = reader.GetString(2)
            }, parameters);
            return type.FirstOrDefault() ?? new MstType();
        }
    }
}
