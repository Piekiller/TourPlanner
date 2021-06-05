using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.PostgresDB
{
    class TourLogPostgresDAO : ITourLogDAO
    {
        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"TourLog\" WHERE \"Id\"=@Id;";
        private const string SQL_GET_ALL_TOURLOGS = "SELECT * FROM public.\"TourLog\" WHERE \"TourId\"=@TourId;";
        private const string SQL_INSERT_NEW_TOURLOG =
            "INSERT INTO public.\"TourLog\" (\"Date\",\"Report\",\"Distance\",\"Time\",\"Rating\",\"Id\",\"AvgSpeed\",\"BurnedJoule\",\"Difficulty\",\"HeightDelta\",\"TourId\",\"MaxSpeed\") " +
            "VALUES (@Date,@Report,@Distance,@Time,@Rating,@Id,@AvgSpeed,@BurnedJoule,@Difficulty,@HeightDelta,@TourId,@MaxSpeed) RETURNING \"Id\";";
        private const string SQL_DELETE_TOURLOG = "DELETE FROM public.\"TourLog\" WHERE \"Id\"=@Id RETURNING \"Id\";";
        private const string SQL_UPDATE_TOURLOG = "UPDATE public.\"TourLog\" SET \"Date\"=@Date,\"Report\"=@Report,\"Distance\"=@Distance,"+
            "\"Time\"=@Time,\"Rating\"=@Rating,\"AvgSpeed\"=@AvgSpeed,\"BurnedJoule\"=@BurnedJoule,\"Difficulty\"=@Difficulty,\"HeightDelta\"=@HeightDelta,\"MaxSpeed\"=@MaxSpeed WHERE \"Id\"=@Id RETURNING \"Id\";";
        private IDatabase _database;
        private ITourDAO _tourDAO;
        public TourLogPostgresDAO()
        {
            _database = DALFactory.GetDatabase();
            _tourDAO = DALFactory.CreateTourDAO();
        }
        public TourLogPostgresDAO(IDatabase database, ITourDAO tourDAO)
        {
            _database = database;
            _tourDAO = tourDAO;
        }
        public async Task<TourLog> AddNewTourLog(TourLog log)
        {
            DbCommand command = _database.CreateCommand(SQL_INSERT_NEW_TOURLOG);
            _database.DefineParameter(command, "@Date", DbType.DateTime, log.Date);
            _database.DefineParameter(command, "@Report", DbType.String, log.Report);
            _database.DefineParameter(command, "@Distance", DbType.Double, log.Distance);
            _database.DefineParameter(command, "@Time", DbType.Time, log.Time);
            _database.DefineParameter(command, "@Rating", DbType.Int32, log.Rating);
            _database.DefineParameter(command, "@Id", DbType.Guid, log.Id);
            _database.DefineParameter(command, "@AvgSpeed", DbType.Double, log.AvgSpeed);
            _database.DefineParameter(command, "@BurnedJoule", DbType.Int32, log.BurnedJoule);
            _database.DefineParameter(command, "@Difficulty", DbType.Int32, log.Difficulty);
            _database.DefineParameter(command, "@HeightDelta", DbType.Int32, log.HeightDelta);
            _database.DefineParameter(command, "@TourId", DbType.Guid, log.Tour.Id);
            _database.DefineParameter(command, "@MaxSpeed", DbType.Double, log.MaxSpeed);
            return await FindById(await _database.ExecuteScalar(command));
        }

        public async Task<TourLog> FindById(Guid id)
        {
            DbCommand command = _database.CreateCommand(SQL_FIND_BY_ID);
            _database.DefineParameter<Guid>(command, "@Id", DbType.Guid, id);
            using IDataReader reader = await _database.ExecuteReader(command);
            return new TourLog(
                (DateTime)reader["Date"], (string)reader["Report"], (double)reader["Distance"], 
                (TimeSpan)reader["Time"], (int)reader["Rating"],  (double)reader["AvgSpeed"], 
                (int)reader["BurnedJoule"], (int)reader["Difficulty"], (int)reader["HeightDelta"], 
                await _tourDAO.FindById((Guid)reader["TourId"]), (int)reader["MaxSpeed"], (Guid)reader["Id"]);
        }

        public async Task<IEnumerable<TourLog>> GetLogForTour(Tour tour)
        {
            DbCommand command = _database.CreateCommand(SQL_GET_ALL_TOURLOGS);
            _database.DefineParameter(command, "@TourId", DbType.Guid, tour.Id);
            using IDataReader reader = await _database.ExecuteReader(command);
            List<TourLog> tourlogs = new();
            while (reader.Read())
            {
                Tour tour1 = await _tourDAO.FindById((Guid)reader["TourId"]);
                tourlogs.Add(new TourLog(
                (DateTime)reader["Date"], (string)reader["Report"], (double)reader["Distance"],
                (TimeSpan)reader["Time"], (int)reader["Rating"], (double)reader["AvgSpeed"],
                (int)reader["BurnedJoule"], (int)reader["Difficulty"], (int)reader["HeightDelta"],
                tour1, (double)reader["MaxSpeed"], (Guid)reader["Id"]));
            }
                
            return tourlogs;

        }
        public async Task DeleteTourLog(Guid id)
        {
            DbCommand command = _database.CreateCommand(SQL_DELETE_TOURLOG);
            _database.DefineParameter<Guid>(command, "@Id", DbType.Guid, id);
            await _database.ExecuteScalar(command);
        }

        public async Task UpdateTourLog(TourLog log)
        {
            DbCommand command = _database.CreateCommand(SQL_UPDATE_TOURLOG);
            _database.DefineParameter(command, "@Date", DbType.DateTime, log.Date);
            _database.DefineParameter(command, "@Report", DbType.String, log.Report);
            _database.DefineParameter(command, "@Distance", DbType.Double, log.Distance);
            _database.DefineParameter(command, "@Time", DbType.Time, log.Time);
            _database.DefineParameter(command, "@Rating", DbType.Int32, log.Rating);
            _database.DefineParameter(command, "@AvgSpeed", DbType.Double, log.AvgSpeed);
            _database.DefineParameter(command, "@BurnedJoule", DbType.Int32, log.BurnedJoule);
            _database.DefineParameter(command, "@Difficulty", DbType.Int32, log.Difficulty);
            _database.DefineParameter(command, "@HeightDelta", DbType.Int32, log.HeightDelta);
            _database.DefineParameter(command, "@TourId", DbType.Guid, log.Tour.Id);
            _database.DefineParameter(command, "@MaxSpeed", DbType.Double, log.MaxSpeed);
            _database.DefineParameter(command, "@Id", DbType.Double, log.Id);
            await _database.ExecuteScalar(command);
        }
    }
}
