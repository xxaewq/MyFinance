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
    public class SqlServerMstTypeRepository(SqlServerDatabaseHelper helper) : IMstTypeRepository
    {
        private readonly SqlServerDatabaseHelper _helper = helper;

        public async Task<bool> CreateType(MstType type, CancellationToken token)
        {
            string sql = "INSERT INTO MstType (Id, TypeName, Description, Enable, CreateAt, CreateBy) " +
                "VALUES (@Id, @TypeName, @Description, @Enable, @CreateAt, @CreateBy)";
            var parameters = new SqlParameter[]
            {
                new("@Id", type.Id),
                new("@TypeName", type.TypeName),
                new("@Description", type.Description),
                new("@Enable", type.Enable),
                new("@CreateAt", type.CreateAt),
                new("@CreateBy", type.CreatedBy)
            };
            return await _helper.ExecuteNonQuery(sql, token, parameters) > 0;
        }

        public async Task<bool> DeleteType(Guid id,string deleteBy, CancellationToken token)
        {
            string sql = "UPDATE MstType SET Enable = @Enable, DeleteBy = @DeleteBy, DeleteAt = @DeleteAt WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new("@Id", id),
                new("@Enable", false),
                new("@DeleteBy", deleteBy),
                new("@DeleteAt", DateTime.Now)
            };
            return await _helper.ExecuteNonQuery(sql, token, parameters) > 0;
        }

        public async Task<List<MstType>> GetAllTypes(CancellationToken token)
        {
            string sql = "SELECT Id, TypeName, Description FROM MstType where Enable = 'true'";
            var types = await _helper.ExecuteQuery(sql, reader => new MstType
            {
                Id = reader.GetGuid(0),
                TypeName = reader.GetString(1),
                Description = reader.GetString(2)
            }, token);
            return types;
        }

        public async Task<MstType?> GetTypeById(Guid id, CancellationToken token)
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
            return type.FirstOrDefault();
        }

        public async Task<bool> UpdateType(MstType type, CancellationToken token)
        {
            string sql = "UPDATE MstType SET TypeName = @TypeName, Description = @Description, UpdateBy = @UpdateBy, UpdateAt = @UpdateAt WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new("@Id", type.Id),
                new("@TypeName", type.TypeName),
                new("@Description", type.Description),
                new("@UpdateBy", type.UpdatedBy),
                new("@UpdateAt", type.UpdateAt)
            };
            return await _helper.ExecuteNonQuery(sql, token, parameters) > 0;
        }
    }
}
