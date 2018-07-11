using System;
using FluentAssertions;
using NUnit.Framework;
using StoreProcedureRepository.Services;

namespace StoredProcedureRepository.UnitTests.ServicesTests
{
    [TestFixture]
    public class GuardTests
    {
        [Test]
        public void ThrowIfNull_NullArgument_ThrowsArgumentNullException()
        {
            //arrange
            object input = null;

            //act
            Action result = () => Guard.ThrowIfNull(input);

            //assert

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ThrowIfStringNullOrEmpty_EmptyString_ThrowsArgumentNullException()
        {
            //arrange
            var input = string.Empty;

            //act
            Action result = () => Guard.ThrowIfStringNullOrEmpty(input);

            //assert

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ThrowIfStringNullOrWhiteSpace_WhiteSpace_ThrowsArgumentNullException()
        {
            //arrange
            var input = " ";

            //act
            Action result = () => Guard.ThrowIfStringNullOrWhiteSpace(input);

            //assert

            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ThrowIfAnyIsNullOrEmpty_OneStringIsEmpty_ThrowsArgumentException()
        {
            //arrange
            var input = new[] { "Test", string.Empty };

            //act
            Action result = () => Guard.ThrowIfAnyIsNullOrEmpty(input);

            //assert

            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ThrowIfEmpty_EmptyList_ThrowsArgumentException()
        {
            //arrange
            var input = new string[] { };

            //act
            Action result = () => Guard.ThrowIfEmpty(input);

            //assert

            result
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("entities cannot be empty.");
        }
    }
}
