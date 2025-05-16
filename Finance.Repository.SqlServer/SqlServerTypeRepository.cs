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
    public class SqlServerTypeRepository(SqlServerDatabaseHelper helper) : ITypeRepository
    {
        private readonly SqlServerDatabaseHelper _helper = helper;

        public async Task<bool> CreateType(MstType type, CancellationToken token)
        {
            string sql = "INSERT INTO MstType (Id, TypeName, Description) VALUES (@Id, @TypeName, @Description)";
            var parameters = new SqlParameter[]
            {
                new("@Id", type.Id),
                new("@TypeName", type.TypeName),
                new("@Description", type.Description)
            };
            return await _helper.ExecuteNonQuery(sql, token, parameters) > 0;
        }

        public async Task<bool> DeleteType(Guid id, CancellationToken token)
        {
            string sql = "DELETE FROM MstType WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new("@Id", id)
            };
            return await _helper.ExecuteNonQuery(sql, token, parameters) > 0;
        }

        public async Task<List<MstType>> GetAllTypes(CancellationToken token)
        {
            string sql = "SELECT * FROM MstType";
            var types = await _helper.ExecuteQuery(sql, reader => new MstType
            {
                Id = reader.GetGuid(0),
                TypeName = reader.GetString(1),
                Description = reader.GetString(2)
            }, token);
            return types;
        }

        public async Task<MstType> GetTypeById(Guid id, CancellationToken token)
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
            }, token, parameters);
            return type.FirstOrDefault() ?? new MstType();
        }

        public async Task<bool> UpdateType(MstType type, CancellationToken token)
        {
            string sql = "UPDATE MstType SET TypeName = @TypeName, Description = @Description WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new("@Id", type.Id),
                new("@TypeName", type.TypeName),
                new("@Description", type.Description)
            };
            return await _helper.ExecuteNonQuery(sql,token, parameters) > 0;
        }
    }
}
