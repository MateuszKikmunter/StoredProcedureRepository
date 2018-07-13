using System;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.Infrastructure.Services;
using StoredProcedureRepository.UnitTests.Helpers;

namespace StoredProcedureRepository.UnitTests.ServicesTests
{
    [TestFixture]
    public class StoredProcedureNameFormatterTests
    {
        [Test]
        public void GetStoredProcedureNameWithParameters_SpNameEmptyString_ThrowsArgumentNullException()
        {
            //arrange
            var spName = string.Empty;
            var param = new { };

            //act
            Action result = () => StoredProcedureNameFormatter.GetStoredProcedureNameWithParameters(spName, param);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetStoredProcedureNameWithParameters_ValidInput_ReturnsSpNameWithParameterNames()
        {
            //arrange
            var spName = "StoredProcedureName";
            var parameterValue = new FakeEntity
            {
                Id = 1,
                Name = "Name"
            };
            //act
            var result = StoredProcedureNameFormatter.GetStoredProcedureNameWithParameters(spName, parameterValue);

            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain($"@{parameterValue.Name}");
            result.Should().Contain("@Id");
            result.Should().Be("StoredProcedureName @Id, @Name");
        }
    }
}
