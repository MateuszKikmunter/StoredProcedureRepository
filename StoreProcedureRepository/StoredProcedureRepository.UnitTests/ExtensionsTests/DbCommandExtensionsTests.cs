using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.Infrastructure.Extensions;
using StoredProcedureRepository.UnitTests.Helpers;

namespace StoredProcedureRepository.UnitTests.ExtensionsTests
{
    [TestFixture]
    public class DbCommandExtensionsTests
    {
        private SqlCommand _command;

        [SetUp]
        public void SetUp() => _command = new SqlCommand { CommandText = "command text" };

        [TearDown]
        public void TearDown() => _command.Dispose();

        [Test]
        public void WithSqlParam_ParamNameEmptyString_ThrowsArgumentException()
        {
            //arrange
            var paramName = string.Empty;
            var paramValue = new { };

            //act
            Action result = () => _command.WithSqlParam(paramName, paramValue);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void WithSqlParam_CommandTextEmptyString_ThrowsArgumentException()
        {
            //arrange
            var paramName = "ParamName";
            object paramValue = null;
            _command.CommandText = string.Empty;

            //act
            Action result = () => _command.WithSqlParam(paramName, paramValue);

            //act
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void WithSqlParam_ParamValueNull_ThrowsArgumentNullException()
        {
            //arrange
            var paramName = "ParamName";
            object paramValue = null;

            //act
            Action result = () => _command.WithSqlParam(paramName, paramValue);

            //act
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void WithSqlParam_NameValueAndCommandTextCorrect_CreatesSqlParameterForCommand()
        {
            //arrange
            var paramName = "ParamName";
            var paramValue = "ParamValue";

            //act
            _command.WithSqlParam(paramName, paramValue);
            var addedParam = _command.Parameters[0];

            //assert
            addedParam.ParameterName.Should().Be($"@{paramName}");
            addedParam.Value.Should().Be(paramValue);
        }

        [Test]
        public void WithSqlParam_MultipleParameters_AddsSqlParametersToCommand()
        {
            //arrange
            var firstParam = new { Name = "FirstParam", Value = 1 };
            var secondParam = new { Name = "SecondParam", Value = 2 };

            //act
            _command
                .WithSqlParam(firstParam.Name, firstParam.Value)
                .WithSqlParam(secondParam.Name, secondParam.Value);

            var firstAddedParam = _command.Parameters[0];
            var secondAddedParam = _command.Parameters[1];

            //assert
            _command.Parameters.Count.Should().Be(2);

            firstAddedParam.ParameterName.Should().Be($"@{firstParam.Name}");
            firstAddedParam.Value.Should().Be(firstParam.Value);

            secondAddedParam.ParameterName.Should().Be($"@{secondParam.Name}");
            secondAddedParam.Value.Should().Be(secondParam.Value);
        }

        [Test]
        public void WithSqlParam_WithConfigForOutputParameter_AddsSqlParameterToCommand()
        {
            //act
            _command.WithSqlParam("ParamName", "Param", parameter =>
            {
                parameter.Direction = ParameterDirection.Output;
                parameter.DbType = DbType.String;
                parameter.Size = 100;
            });

            var addedParameter = _command.Parameters[0];

            //assert
            _command.Parameters.Count.Should().Be(1);

            addedParameter.ParameterName.Should().Be("@ParamName");
            addedParameter.Value.Should().Be("Param");
            addedParameter.DbType.Should().Be(DbType.String);
            addedParameter.Size.Should().Be(100);
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_ParamNameEmptyString_ThrowsArgumentNullException()
        {
            //arrange
            var paramName = string.Empty;
            var paramValue = new List<FakeEntity>();

            //act
            Action result = () => _command.WithUserDefinedDataTableSqlParam(paramName, paramValue);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_ParamValueNull_CreatesParameterForCommand()
        {
            //arrange
            var paramName = "FakeParameter";
            var paramValue = new List<FakeEntity>
            {
                new FakeEntity
                {
                    Id = 1, Name = "Fake1"
                }
            };

            //act
            _command.WithUserDefinedDataTableSqlParam(paramName, paramValue);
            var addedParam = _command.Parameters[0];
            var addedParamValueDataTable = addedParam.Value as DataTable;

            //assert
            _command.Parameters.Count.Should().Be(1);
            addedParam.SqlDbType.Should().Be(SqlDbType.Structured);
            addedParam.ParameterName.Should().Be($"@{paramName}");

            addedParamValueDataTable.Rows.Count.Should().Be(1);
            addedParamValueDataTable.Rows[0].ItemArray.Length.Should().Be(2);
            addedParamValueDataTable.Rows[0].ItemArray[0].Should().Be(1);
            addedParamValueDataTable.Rows[0].ItemArray[1].Should().Be("Fake1");

            addedParamValueDataTable.Columns.Count.Should().Be(2);
            addedParamValueDataTable.Columns[0].ColumnName.Should().Be("Id");
            addedParamValueDataTable.Columns[1].ColumnName.Should().Be("Name");
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_MultipleParametersWithDifferentTypes_CreatesParametersForCommand()
        {
            //arrange
            var firstParameter = new
            {
                Name = "FakeParameter1",
                Value = new List<FakeEntity>
                {
                    new FakeEntity
                    {
                        Id = 1,
                        Name = "Fake1"
                    }
                }
            };

            var secondParameter = new
            {
                Name = "FakeParameter2",
                Value = new List<FakeUser>
                {
                    new FakeUser
                    {
                        Id = 2,
                        Name = "Fake2",
                        Active = true
                    }
                }
            };

            //act
            _command
                .WithUserDefinedDataTableSqlParam(firstParameter.Name, firstParameter.Value)
                .WithUserDefinedDataTableSqlParam(secondParameter.Name, secondParameter.Value);

            var firstAddedParam = _command.Parameters[0];
            var secondAddedParam = _command.Parameters[1];

            var firstAddedParamValueDataTable = firstAddedParam.Value as DataTable;
            var secondAddedParamValueDataTable = secondAddedParam.Value as DataTable;

            //assert
            _command.Parameters.Count.Should().Be(2);

            //first added parameter assertions
            firstAddedParam.SqlDbType.Should().Be(SqlDbType.Structured);
            firstAddedParam.ParameterName.Should().Be($"@{firstParameter.Name}");

            firstAddedParamValueDataTable.Rows.Count.Should().Be(1);
            firstAddedParamValueDataTable.Rows[0].ItemArray.Length.Should().Be(2);
            firstAddedParamValueDataTable.Rows[0].ItemArray[0].Should().Be(1);
            firstAddedParamValueDataTable.Rows[0].ItemArray[1].Should().Be("Fake1");

            firstAddedParamValueDataTable.Columns.Count.Should().Be(2);
            firstAddedParamValueDataTable.Columns[0].ColumnName.Should().Be("Id");
            firstAddedParamValueDataTable.Columns[1].ColumnName.Should().Be("Name");

            //second added parameter assertions
            secondAddedParamValueDataTable.Rows.Count.Should().Be(1);
            secondAddedParamValueDataTable.Rows[0].ItemArray.Length.Should().Be(3);
            secondAddedParamValueDataTable.Rows[0].ItemArray[0].Should().Be(true);
            secondAddedParamValueDataTable.Rows[0].ItemArray[1].Should().Be(2);
            secondAddedParamValueDataTable.Rows[0].ItemArray[2].Should().Be("Fake2");

            secondAddedParamValueDataTable.Columns.Count.Should().Be(3);
            secondAddedParamValueDataTable.Columns[0].ColumnName.Should().Be("Active");
            secondAddedParamValueDataTable.Columns[1].ColumnName.Should().Be("Id");
            secondAddedParamValueDataTable.Columns[2].ColumnName.Should().Be("Name");
        }

        [Test]
        public void WithUserDefinedDataTableSqlParamAndWithSqlParam_MixedParameters_AddsParametersToCommandParametersCollection()
        {
            //arrange
            var userDefinedTableTypeParameter = new
            {
                ParamName = "UserDefinedTableTypeFakeParameter",
                ParamValue = new List<FakeEntity>
                {
                    new FakeEntity
                    {
                        Id = 1, Name = "Fake1"
                    }
                }
            };

            var secondParameter = new
            {
                ParamName = "FakeParameter",
                ParamValue = 1
            };

            //act
            _command
                .WithUserDefinedDataTableSqlParam(userDefinedTableTypeParameter.ParamName, userDefinedTableTypeParameter.ParamValue)
                .WithSqlParam(secondParameter.ParamName, secondParameter.ParamValue);

            var firstAddedParam = _command.Parameters[0];
            var secondAddedParam = _command.Parameters[1];
            var firstAddedParamValueDataTable = firstAddedParam.Value as DataTable;

            //assert
            _command.Parameters.Count.Should().Be(2);

            //first added param assertions
            firstAddedParam.SqlDbType.Should().Be(SqlDbType.Structured);
            firstAddedParam.ParameterName.Should().Be($"@{userDefinedTableTypeParameter.ParamName}");

            firstAddedParamValueDataTable.Rows.Count.Should().Be(1);
            firstAddedParamValueDataTable.Rows[0].ItemArray.Length.Should().Be(2);
            firstAddedParamValueDataTable.Rows[0].ItemArray[0].Should().Be(1);
            firstAddedParamValueDataTable.Rows[0].ItemArray[1].Should().Be("Fake1");

            firstAddedParamValueDataTable.Columns.Count.Should().Be(2);
            firstAddedParamValueDataTable.Columns[0].ColumnName.Should().Be("Id");
            firstAddedParamValueDataTable.Columns[1].ColumnName.Should().Be("Name");

            //second added param assertions
            secondAddedParam.ParameterName.Should().Be($"@{secondParameter.ParamName}");
            secondAddedParam.Value.Should().Be(secondParameter.ParamValue);
        }
    }
}
