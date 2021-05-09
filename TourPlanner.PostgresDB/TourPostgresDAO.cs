using System;
using System.Collections.Generic;
using System.Data.Common;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;
using System.Data;
using System.Threading.Tasks;

namespace TourPlanner.PostgresDB
{
    public class TourPostgresDAO : ITourDAO
    {
        private const string SQL_FIND_BY_ID="SELECT * FROM public.\"Tour\" WHERE \"Id\"=@Id;";
        private const string SQL_GET_ALL_TOURS = "SELECT * FROM public.\"Tour\";";
        private const string SQL_INSERT_NEW_TOUR = "INSERT INTO public.\"Tour\" (\"Name\", \"Description\",\"RouteInformation\",\"Distance\",\"Id\",\"StartPoint\",\"EndPoint\") "+
                                                   "VALUES (@Name,@Description,@RouteInformation,@Distance,@Id,@StartPoint,@EndPoint) RETURNING \"Id\";";
        private const string SQL_DELETE_TOUR = "DELETE FROM public.\"Tour\" WHERE \"Id\"=@Id;";
        private IDatabase _database;
        public TourPostgresDAO()
        {
            _database = DALFactory.GetDatabase();
        }
        public TourPostgresDAO(IDatabase database)
        {
            _database = database;
        }
        public async Task<Tour> AddNewTour(Tour tour)
        {
            DbCommand command = _database.CreateCommand(SQL_INSERT_NEW_TOUR);
            _database.DefineParameter(command, "@Name", DbType.String,tour.Name);
            _database.DefineParameter(command, "@Description", DbType.String, tour.Description);
            _database.DefineParameter(command, "@RouteInformation", DbType.String, tour.RouteInformation);
            _database.DefineParameter(command, "@Distance", DbType.Double, tour.Distance);
            return await FindById(await _database.ExecuteScalar(command));
        }

        public async Task DeleteTour(Guid id)
        {
            DbCommand command = _database.CreateCommand(SQL_DELETE_TOUR);
            _database.DefineParameter<Guid>(command, "@Id", DbType.Guid, id);
            await _database.ExecuteScalar(command);
        }

        public async Task<Tour> FindById(Guid id)
        {
            DbCommand command = _database.CreateCommand(SQL_FIND_BY_ID);
            _database.DefineParameter<Guid>(command, "@Id", DbType.Guid, id);
            using IDataReader reader = await _database.ExecuteReader(command);
            return new Tour(
                (string)reader["Name"], (string)reader["Description"], (string)reader["RouteInformation"],
                (double)reader["Distance"], (string)reader["StartPoint"], (string)reader["EndPoint"], (Guid)reader["Id"]);
        }

        public async Task<IEnumerable<Tour>> GetTours()
        {
            DbCommand command = _database.CreateCommand(SQL_GET_ALL_TOURS);
            using IDataReader reader = await _database.ExecuteReader(command);
            List <Tour> tours= new();
            while (reader.Read())
                tours.Add(new Tour(
                (string)reader["Name"], (string)reader["Description"], (string)reader["RouteInformation"],
                (double)reader["Distance"], (string)reader["StartPoint"], (string)reader["EndPoint"], (Guid)reader["Id"]));
            return tours;
        }

    }
}
