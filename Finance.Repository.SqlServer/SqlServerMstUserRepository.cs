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
    public class SqlServerMstUserRepository(SqlServerDatabaseHelper helper) : IMstUserRepository
    {
        private readonly SqlServerDatabaseHelper _helper = helper;

        public async Task<bool> CreateUserAsync(MstUser user, CancellationToken token)
        {
            string procedure = "sp_CreateUserByProcedure";
            SqlParameter[] parameters =
            [
                new ("@Id", user.Id),
                    new ("@UserName", user.UserName),
                    new ("@FullName", user.FullName),
                    new ("@Password", user.Password),
                    new ("@Enable", user.Enable),
                    new ("@CreateAt", (object?)user.CreateAt ?? DBNull.Value),
                    new ("@CreateBy", (object?)user.CreateBy ?? DBNull.Value)
            ];
            var results = await _helper.ExecuteProcedureAsync(procedure, token, parameters);
            if (results.TryGetValue("Exception", out object? value))
            {
                throw new Exception(value.ToString());
            }
            return true;
        }

        public async Task<List<MstUser>> GetAllUsersAsync(CancellationToken token = default)
        {
            string sql = "SELECT Id, UserName, FullName FROM MstUser WHERE Enable = @Enable ";
            SqlParameter[] parameters =
            [
                new ("@Enable", true)
            ];
            var results = await _helper.ExecuteQueryAsync(sql, reader => new MstUser
            {
                Id = reader.GetGuid(0),
                UserName = reader.GetString(1),
                FullName = reader.GetString(2)
            }, token, parameters);
            return results;
        }

        public Task<MstUser?> GetByIdAsync(Guid id, CancellationToken token)
        {
            string sql = "SELECT Id, UserName, FullName FROM MstUser WHERE Id = @Id AND Enable = @Enable";
            SqlParameter[] parameters =
            [
                new ("@Id", id),
                    new ("@Enable", true)
            ];
            return _helper.ExecuteQuerySingleOrDefaultAsync(sql, reader => new MstUser
            {
                Id = reader.GetGuid(0),
                UserName = reader.GetString(1),
                FullName = reader.GetString(2)
            }, token, parameters);
        }
    }
}
