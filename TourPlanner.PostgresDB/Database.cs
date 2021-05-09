using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;

namespace TourPlanner.PostgresDB
{
    public class Database:IDatabase
    {
        private string _connectionString;
        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbCommand CreateCommand(string CommandText) 
            => new NpgsqlCommand(CommandText);

        public int DeclareParameter(DbCommand command, string name, DbType type)
        {
            if (!command.Parameters.Contains(name))
                command.Parameters.Add(new NpgsqlParameter(name, type));
            throw new ArgumentException($"Parameter {name} already exists.");
        }

        public void DefineParameter<T>(DbCommand command, string name, DbType type, T value)
        {
            int index = DeclareParameter(command, name, type);
            command.Parameters[index].Value = value;
        }
        public void SetParameter<T>(DbCommand command, string name, T value)
        {
            if (command.Parameters.Contains(name))
                command.Parameters[name].Value = value;
            throw new ArgumentException($"Parameter {name} does not exist.");
        }
        public async Task<IDataReader> ExecuteReader(DbCommand command)
        {
            command.Connection = CreateConnection();
            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public async Task<Guid> ExecuteScalar(DbCommand command)
        {
            command.Connection = CreateConnection();
            return Guid.Parse((await command.ExecuteScalarAsync()).ToString());
        }

        private DbConnection CreateConnection()
        {
            DbConnection connection= new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        
    }
}
