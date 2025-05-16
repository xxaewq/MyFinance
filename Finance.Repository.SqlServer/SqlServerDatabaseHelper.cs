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

        private async Task OpenConnectionAsync()
        {
            if(_sqlConnection.State != System.Data.ConnectionState.Open)
            {
                await _sqlConnection.OpenAsync();
            }
        }

        public async Task<List<T>> ExecuteQuery<T>(string query, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
        {

            List<T> results = [];
            try
            {
                await OpenConnectionAsync();
                using SqlCommand command = new(query, _sqlConnection);
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
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

        public async Task<int> ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            int rowsAffected = 0;
            try
            {
                await OpenConnectionAsync();
                using SqlCommand command = new(query, _sqlConnection);
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return rowsAffected;
        }

        public async Task<T> ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            try
            {
                await OpenConnectionAsync();
                using SqlCommand command = new(query, _sqlConnection);
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                var result = await command.ExecuteScalarAsync();
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


        public async Task<Dictionary<string, object>> ExecuteProcedure(string procedureName, params SqlParameter[] parameters)
        {
            Dictionary<string, object> results = [];
            try
            {
                await OpenConnectionAsync();
                using SqlCommand command = new(procedureName, _sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
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

        public async Task<bool> ExecuteTransaction(List<(string, SqlParameter[] parameters)> queries)
        {
            await OpenConnectionAsync();
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
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
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
