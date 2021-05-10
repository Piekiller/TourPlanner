using NUnit.Framework;
using System;
using System.Data.Common;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.PostgresDB;
using Moq;
namespace TourPlanner.Tests
{
    public class DatabaseMock
    {
        [Test]
        public void Test_declare_parameter_throw_ArgumentException()
        {
            IDatabase db = new Database("");
            DbCommand command = db.CreateCommand("");
            db.DeclareParameter(command, "test", System.Data.DbType.String);
            Assert.Throws(typeof(ArgumentException), () => db.DeclareParameter(command, "test", System.Data.DbType.String));
        }
        [Test]
        public void Test_set_parameter_throw_ArgumentException()
        {
            IDatabase db = new Database("");
            DbCommand command = db.CreateCommand("");
            Assert.Throws(typeof(ArgumentException), () => db.SetParameter(command, "test", System.Data.DbType.String));
        }

    }
}