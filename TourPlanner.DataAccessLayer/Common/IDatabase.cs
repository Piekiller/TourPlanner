using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer.Common
{
    public interface IDatabase
    {
        DbCommand CreateCommand(string CommandText);
        int DeclareParameter(DbCommand command, string name, DbType type);
        void DefineParameter<T>(DbCommand command, string name, DbType type, T value);
        void SetParameter<T>(DbCommand command, string name, T value);
        Task<IDataReader> ExecuteReader(DbCommand command);
        Task<Guid> ExecuteScalar(DbCommand command);
    }
}
