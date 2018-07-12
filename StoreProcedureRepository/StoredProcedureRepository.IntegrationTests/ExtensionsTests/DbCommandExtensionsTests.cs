using System.Data;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.Infrastructure.Extensions;
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
            SqlScriptRunner.SetUpDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            SqlScriptRunner.ClearDatabase();
        }

        [Test]
        public void LoadStoredProcedure_LoadsdbCommand()
        {
            //arrange
            var storedProcedureName = "HighPerformanStoredProcedure";

            //act
            var command = _context.LoadStoredProcedure(storedProcedureName);

            //assert
            command.Should().NotBeNull();
            command.CommandText.Should().Be(storedProcedureName);
            command.CommandType.Should().Be(CommandType.StoredProcedure);
        }
    }
}
