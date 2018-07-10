using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.Infrastructure.Services;
using StoredProcedureRepository.UnitTests.Helpers;

namespace StoredProcedureRepository.UnitTests.ServicesTests
{
    [TestFixture]
    public class SqlParameterFactoryTests
    {
        [Test]
        public void CreateParameter_ParameterNameStringEmpty_ThrowsException()
        {
            //arrange
            var paramName = string.Empty;
            var paramValue = new { };

            //act
            Action result = () => SqlParameterFactory.CreateParameter(paramName, paramValue);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CreateParameter_ParameterValueIsNull_ThrowsException()
        {
            //arrange
            var paramName = "Param";
            object paramValue = null;

            //act
            Action result = () => SqlParameterFactory.CreateParameter(paramName, paramValue);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CreateParameter_CorrectInput_CreatesSqlParameter()
        {
            //arrange
            var paramName = "Param";
            var paramValue = 123;

            //act
            var result = SqlParameterFactory.CreateParameter(paramName, paramValue);

            //assert
            result.Should().NotBeNull();
            result.ParameterName.Should().Be($"@{paramName}");
            result.Value.Should().Be(paramValue);
        }

        [Test]
        public void CreateParameter_EmptyString_ThrowsException()
        {
            //arrange
            var paramName = string.Empty;

            //act
            Action result = () => SqlParameterFactory.CreateParameter(paramName);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CreateParameter_ValidParameterName_CreatesSqlParameter()
        {
            //arrange
            var paramName = "Param";

            //act
            var result = SqlParameterFactory.CreateParameter(paramName);

            //assert
            result.Should().NotBeNull();
            result.ParameterName.Should().Be($"@{paramName}");
        }

        [Test]
        public void BuildParamsForObject_NullObject_ThrowsException()
        {
            //arrange
            object paramValue = null;

            //act
            Action result = () => SqlParameterFactory.BuildParamsForObject(paramValue);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void BuildParamsForObject_ValidObject_CreatesSqlParameterForEveryObjectProperty()
        {
            //arrange
            var param = new { Id = 1, Name = "Test" };

            //act
            var result = SqlParameterFactory.BuildParamsForObject(param);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<SqlParameter[]>();
            result.Length.Should().Be(2);

            result.First().ParameterName.Should().Be("@Id");
            result.First().Value.Should().Be(param.Id);

            result.Last().ParameterName.Should().Be("@Name");
            result.Last().Value.Should().Be(param.Name);
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_NullValue_ThrowsException()
        {
            //arrange
            var paramName = "Test";
            IList<FakeEntity> paramValue = null;

            //act
            Action result = () => SqlParameterFactory.BuildUserDefinedTableTypeParameter(paramName, paramValue);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_EmptyCollectionAsParamValue_ThrowsException()
        {
            //arrange
            var paramName = string.Empty;
            var paramValue = new List<FakeEntity>();

            //act
            Action result = () => SqlParameterFactory.BuildUserDefinedTableTypeParameter(paramName, paramValue);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_CorrectInput_CreatesParameter()
        {
            //arrange
            var paramName = "Test";
            IList<FakeEntity> paramValue = new List<FakeEntity>
            {
                new FakeEntity { Id = 1, Name = "Fake1"}
            };

            //act
            var result = SqlParameterFactory.BuildUserDefinedTableTypeParameter(paramName, paramValue);
            var parameterValue = result.Value as DataTable;

            //assert
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.SqlDbType.Should().Be(SqlDbType.Structured);

            parameterValue.Rows.Count.Should().Be(1);

            parameterValue.Columns.Count.Should().Be(2);
            parameterValue.Columns[0].ColumnName.Should().Be("Id");
            parameterValue.Columns[1].ColumnName.Should().Be("Name");
        }
    }
}
