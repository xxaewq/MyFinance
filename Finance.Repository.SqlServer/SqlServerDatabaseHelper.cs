using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Repository.SqlServer
{
    public class SqlServerDatabaseHelper : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _sqlConnection;
        private bool disposedValue;

        public SqlServerDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
            _sqlConnection = new SqlConnection(_connectionString);
        }

        private async Task OpenConnectionAsync(CancellationToken token)
        {
            if(_sqlConnection.State != System.Data.ConnectionState.Open)
            {
                await _sqlConnection.OpenAsync(token);
            }
        }

        public async Task<List<T>> ExecuteQuery<T>(string query, Func<SqlDataReader, T> mapFunction, CancellationToken token, params SqlParameter[] parameters)
        {
            List<T> results = [];
            try
            {
                await OpenConnectionAsync(token);
                using SqlCommand command = new(query, _sqlConnection);
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                using SqlDataReader reader = await command.ExecuteReaderAsync(token);
                while (await reader.ReadAsync(token))
                {
                    results.Add(mapFunction(reader));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        public async Task<int> ExecuteNonQuery(string query, CancellationToken token, params SqlParameter[] parameters)
        {
            int rowsAffected = 0;
            try
            {
                await OpenConnectionAsync(token);
                using SqlCommand command = new(query, _sqlConnection);
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                rowsAffected = await command.ExecuteNonQueryAsync(token);
            }
            catch (Exception)
            {
                throw;
            }
            return rowsAffected;
        }

        public async Task<T> ExecuteScalar<T>(string query, CancellationToken token, params SqlParameter[] parameters)
        {
            try
            {
                await OpenConnectionAsync(token);
                using SqlCommand command = new(query, _sqlConnection);
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                var result = await command.ExecuteScalarAsync(token);
                if (result != null && result != DBNull.Value)
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return default!;
        }


        public async Task<Dictionary<string, object>> ExecuteProcedure(string procedureName, CancellationToken token, params SqlParameter[] parameters)
        {
            Dictionary<string, object> results = [];
            try
            {
                await OpenConnectionAsync(token);
                using SqlCommand command = new(procedureName, _sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                using SqlDataReader reader = await command.ExecuteReaderAsync(token);
                while (await reader.ReadAsync(token))
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        results[reader.GetName(i)] = reader.GetValue(i);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        public async Task<bool> ExecuteTransaction(List<(string, SqlParameter[] parameters)> queries, CancellationToken token)
        {
            await OpenConnectionAsync(token);
            using SqlTransaction transaction = _sqlConnection.BeginTransaction();
            try
            {
                foreach (var (query, parameters) in queries)
                {
                    using SqlCommand command = new(query, _sqlConnection, transaction);
                    if(parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    await command.ExecuteReaderAsync(token);
                }
                await transaction.CommitAsync(token);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if(_sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        _sqlConnection.Close();
                        _sqlConnection.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SqlServerDatabaseHelper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
