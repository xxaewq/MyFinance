using Finance.Repository.Abstraction;
using Finance.Repository.Abstraction.Entities;
using Microsoft.Data.SqlClient;

namespace Finance.Repository.SqlServer;

public class SqlServerMstUserRepository(SqlServerDatabaseHelper helper) : IMstUserRepository
{
    private readonly SqlServerDatabaseHelper _helper = helper;

    public async Task<int> CheckPasswordAsync(string username, string password, CancellationToken token)
    {
        string functionName = "SELECT dbo.fn_CheckPassword(@Username, @Password)";
        SqlParameter[] parameters =
        [
            new ("@Username", username),
            new ("@Password", password)
        ];
        return await _helper.ExecuteScalarAsync<int>(functionName, token, parameters);
    }

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
        if (results.TryGetValue("Exception", out _))
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id, string deletedBy, CancellationToken token)
    {
        string sql = "UPDATE MstUser SET Enable = @Enable, DeleteAt = @DeleteAt, DeleteBy = @DeleteBy WHERE Id = @Id";
        SqlParameter[] parameters =
        [
            new ("@Id", id),
            new ("@Enable", false),
            new ("@DeleteAt", DateTime.Now),
            new ("@DeleteBy", deletedBy)
        ];
        return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
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

    public async Task<bool> UpdateInforAsync(MstUser user, CancellationToken token)
    {
        string sql = "UPDATE MstUser SET FullName = @FullName, UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE Id = @Id AND UserName = @UserName AND Enable = @Enable";
        SqlParameter[] parameters =
        [
            new ("@Id", user.Id),
            new ("@UserName", user.UserName),
            new ("@FullName", user.FullName),
            new ("@UpdateAt", (object?)user.UpdateAt ?? DBNull.Value),
            new ("@UpdateBy", (object?)user.UpdatedBy ?? DBNull.Value),
            new ("@Enable", true)
        ];
        return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
    }

    public async Task<bool> UpdatePasswordAsync(MstUser user, CancellationToken token)
    {
        string sql = "UPDATE MstUser SET Password = @Password, UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE Id = @Id AND Enable = @Enable";
        SqlParameter[] parameters =
        [
            new ("@Id", user.Id),
            new ("@Password", user.Password),
            new ("@UpdateAt", (object?)user.UpdateAt ?? DBNull.Value),
            new ("@UpdateBy", (object?)user.UpdatedBy ?? DBNull.Value),
            new ("@Enable", true)
        ];
        return await _helper.ExecuteNonQueryAsync(sql, token, parameters) > 0;
    }
}
