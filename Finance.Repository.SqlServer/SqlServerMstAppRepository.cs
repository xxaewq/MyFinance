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
    public class SqlServerMstAppRepository(SqlServerDatabaseHelper helper) : IMstAppRepository
    {
        private readonly SqlServerDatabaseHelper _helper = helper;

        public async Task<bool> CreateAsync(MstApp app, CancellationToken token)
        {
            string sql = "INSERT INTO MstApp (Id, TypeApp, NameApp, Description, Enable, CreateAt, CreateBy) " +
                "VALUES (@Id, @TypeApp, @NameApp, @Description, @Enable, @CreateAt, @CreateBy)";
            var parameters = new SqlParameter[]
            {
                new("@Id", app.Id),
                new("@TypeApp", app.TypeApp),
                new("@NameApp", app.NameApp),
                new("@Description", app.Description),
                new("@Enable", true),
                new("@CreateAt", app.CreateAt),
                new("@CreateBy", app.CreateBy)
            };
            return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, string deletedBy, CancellationToken token)
        {
            string sql = "UPDATE MstApp SET Enable = @Enable, DeleteAt = @DeleteAt, DeleteBy = @DeleteBy WHERE Id = @Id";
            var parameters = new SqlParameter[]
            {
                new("@Id", id),
                new("@Enable", false),
                new("@DeleteAt", DateTime.Now),
                new("@DeleteBy", deletedBy)
            };
            return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
        }

        public async Task<List<MstApp>> GetAllAsync(CancellationToken token)
        {
            string sql = "SELECT Id, TypeApp, NameApp, Description FROM MstApp WHERE Enable = @Enable";
            var parameters = new SqlParameter[]
            {
                new("@Enable", true)
            };
            var result = await _helper.ExecuteQueryAsync(sql, reader => new MstApp
            {
                Id = reader.GetGuid(0),
                TypeApp = reader.GetString(1),
                NameApp = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3)
            }, token);

            return result;
        }

        public async Task<MstApp?> GetByIdAsync(Guid id, CancellationToken token)
        {
            string sql = "SELECT Id, TypeApp, NameApp, Description FROM MstApp WHERE Id = @Id AND Enable = @Enable";
            var parameters = new SqlParameter[]
            {
                new("@Id", id),
                new("@Enable", true)
            };
            var result = await _helper.ExecuteQuerySingleOrDefaultAsync(sql, reader => new MstApp
            {
                Id = reader.GetGuid(0),
                TypeApp = reader.GetString(1),
                NameApp = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3)
            }, token, parameters);
            return result;
        }

        public async Task<bool> UpdateAsync(MstApp app, CancellationToken token)
        {
            string sql = "UPDATE MstApp SET TypeApp = @TypeApp, NameApp = @NameApp, Description = @Description, " +
                "UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE Id = @Id and Enable = @Enable";
            var parameters = new SqlParameter[]
            {
                new("@Id", app.Id),
                new("@TypeApp", app.TypeApp),
                new("@NameApp", app.NameApp),
                new("@Description", app.Description),
                new("@UpdateAt", app.UpdateAt),
                new("@UpdateBy", app.UpdateBy),
                new("@Enable", true)
            };
            return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
        }
    }
}
