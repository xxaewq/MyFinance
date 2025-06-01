using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.UserBalance;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.SqlServer
{
    public class SqlServerUserBalanceRepository(SqlServerDatabaseHelper helper) : IUserBalanceRepository
    {
        private readonly SqlServerDatabaseHelper _helper = helper;

        public async Task<bool> CreateUserBalanceAsync(UserBalance userBalance, CancellationToken token)
        {
            string sql = "INSERT INTO dbo.UserBalance (Id, UserId, AppId, Balance, Enable, CreateAt, CreateBy) " +
                "VALUES (@Id, @UserId, @AppId, @Balance, @Enable, @CreateAt, @CreateBy)";
            SqlParameter[] parameters =
                [
                    new SqlParameter("@Id", userBalance.Id),
                    new SqlParameter("@UserId", userBalance.UserId),
                    new SqlParameter("@AppId", userBalance.AppId),
                    new SqlParameter("@Balance", userBalance.Balance),
                    new SqlParameter("@Enable", userBalance.Enable),
                    new SqlParameter("@CreateAt", userBalance.CreateAt ?? (object)DBNull.Value),
                    new SqlParameter("@CreateBy", userBalance.CreateBy ?? (object)DBNull.Value)
                ];
            return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
        }

        public async Task<List<UserBalanceModel>> GetByUserIdAsync(Guid userId, CancellationToken token)
        {
            string sql = "SELECT ub.Id, ub.UserId, mu.UserName, ub.AppId, ma.NameApp, ub.Balance " +
                "FROM dbo.UserBalance AS ub " +
                "INNER JOIN dbo.MstApp AS ma ON ub.AppId = ma.Id AND ma.Enable = 1 " +
                "INNER JOIN dbo.MstUser AS mu ON ub.UserId = mu.Id AND mu.Enable = 1" +
                "WHERE ub.UserId = @UserId AND ub.Enable = 1";
            SqlParameter[] parameters =
                [
                    new SqlParameter("@UserId", userId)
                ];
            return await _helper.ExecuteQueryAsync(sql, reader => new UserBalanceModel()
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                UserName = reader.GetString(2),
                AppId = reader.GetGuid(3),
                AppName = reader.GetString(4),
                Balance = reader.GetDouble(5)
            }, token, parameters);
        }
    }
}
