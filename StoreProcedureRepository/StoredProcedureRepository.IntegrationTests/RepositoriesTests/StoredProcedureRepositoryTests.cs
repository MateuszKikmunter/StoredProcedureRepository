using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.IntegrationTests.Entities;
using StoredProcedureRepository.IntegrationTests.Helpers;

namespace StoredProcedureRepository.IntegrationTests.RepositoriesTests
{
    [TestFixture]
    public class StoredProcedureRepositoryTests
    {
        private Infrastructure.Data.StoredProcedureRepository _repository;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            _repository = new Infrastructure.Data.StoredProcedureRepository(_context);
            SqlScriptRunner.ClearDatabase();
            SqlScriptRunner.SetUpDatabase();
            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _repository = null;
            SqlScriptRunner.ClearDatabase();
        }

        [Test]
        public void Get_GetEmployeeByName_ReturnsEntities()
        {
            //arrange
            var input = new { EmployeeName = "Luke Skywalker" };
            var spName = "GetEmployeeByName";

            //act
            var result = _repository.Get<Employee>(spName, input);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be(input.EmployeeName);
        }

        [Test]
        public void Get_GetAllEmployees_ReturnsEntities()
        {
            //arrange
            var spName = "GetAllEmployees";

            //act
            var result = _repository.Get<Employee>(spName);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(2);
        }

        [Test]
        public void ExecuteCommand_DeleteEmployeeById_ReturnsNumberOfRowsAffected()
        {
            //arrange
            var spName = "DeleteEmployeeById";

            //act

            var empId = _repository
                .Get<Employee>("GetEmployeeByName", new { EmployeeName = "Luke Skywalker" })
                .First()
                .Id;

            var result = _repository.ExecuteCommand(spName, new { EmployeeId = empId });

            //assert
            result.Should().Be(1);
        }

        private void SeedDatabase()
        {
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

            _context.Employees.AddRange(entitiesToInsert);
            _context.SaveChanges();
        }
    }
}
