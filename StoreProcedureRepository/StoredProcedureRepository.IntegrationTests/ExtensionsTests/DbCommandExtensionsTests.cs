using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.Infrastructure.Extensions;
using StoredProcedureRepository.IntegrationTests.Entities;
using StoredProcedureRepository.IntegrationTests.Helpers;

namespace StoredProcedureRepository.IntegrationTests.ExtensionsTests
{
    [TestFixture]
    public class DbCommandExtensionsTests
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            SqlScriptRunner.ClearDatabase();
            SqlScriptRunner.SetUpDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            SqlScriptRunner.ClearDatabase();
        }

        [Test]
        public void LoadStoredProcedure_LoadsDbCommand()
        {
            //arrange
            var storedProcedureName = "HighPerformantStoredProcedure";

            //act
            var command = _context.LoadStoredProcedure(storedProcedureName);

            //assert
            command.Should().NotBeNull();
            command.CommandText.Should().Be(storedProcedureName);
            command.CommandType.Should().Be(CommandType.StoredProcedure);
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_ExecutesStoredProcedureAndReturnsNumberOfAffectedRow()
        {
            //arrange
            var spName = "CreateEmployees";
            var entitiesToInsert = new List<Employee>
            {
                new Employee
                {
                    Name = "Luke Skywalker"
                },
                new Employee
                {
                    Name = "Darth Vader"
                }
            };

            //act
            var cmd = _context
                .LoadStoredProcedure(spName)
                .WithUserDefinedDataTableSqlParam("Employees", entitiesToInsert);

            var result = cmd.ExecuteStoredProceure();

            //assert
            result.Should().Be(2);
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_ExecutesStoredProcedureAndUpdatesEntities()
        {
            //arrange
            var nameToSet = "Boba Fett";
            var entitiesToInsert = new List<Employee>
            {
                new Employee
                {
                    Name = "Luke Skywalker"
                },
                new Employee
                {
                    Name = "Darth Vader"
                }
            };

            //act
            _context
                .LoadStoredProcedure("CreateEmployees")
                .WithUserDefinedDataTableSqlParam("Employees", entitiesToInsert)
                .ExecuteStoredProceure();

            var entitiesFromDataStore = _context
                .LoadStoredProcedure("GetAllEmployees")
                .ExecuteStoredProcedure<Employee>();

            entitiesFromDataStore.ToList().ForEach(e => e.Name = nameToSet);

            var numberOfRowsAffected = _context
                .LoadStoredProcedure("UpdateEmployees")
                .WithUserDefinedDataTableSqlParam("Employees", entitiesFromDataStore)
                .ExecuteStoredProceure();

            //assert
            numberOfRowsAffected.Should().Be(2);
        }

        [Test]
        public void WithSqlParam_ExecutesStoredProcedureAndReturnsEntities()
        {
            //arrange
            var entitiesToInsert = new List<Employee>
            {
                new Employee
                {
                    Name = "Luke Skywalker"
                },
                new Employee
                {
                    Name = "Darth Vader"
                }
            };

            var name = entitiesToInsert.First().Name;

            //act
            _context
                .LoadStoredProcedure("CreateEmployees")
                .WithUserDefinedDataTableSqlParam("Employees", entitiesToInsert)
                .ExecuteStoredProceure();

            var result = _context
                .LoadStoredProcedure("GetEmployeeByName")
                .WithSqlParam("EmployeeName", name)
                .ExecuteStoredProcedure<Employee>();

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(1);
            result.First().Name.Should().Be(name);
        }
    }
}
