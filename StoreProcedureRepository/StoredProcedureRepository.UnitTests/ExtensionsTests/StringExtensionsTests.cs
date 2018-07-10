using System;
using FluentAssertions;
using NUnit.Framework;
using StoredProcedureRepository.Infrastructure.Extensions;

namespace StoredProcedureRepository.UnitTests.ExtensionsTests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void RemoveLastCharacter_CorrectInput_RetursStringWithoutLastCharacter()
        {
            //arrage
            var input = "Test.";
            var result = string.Empty;

            //act
            result = input.RemoveLastCharacter();

            //act
            result.Should().Be("Test");
        }

        [Test]
        public void RemoveLastCharacter_EmptyString_ThrowsNullException()
        {
            //arrage
            var input = string.Empty;

            //act
            Action result = () => input.RemoveLastCharacter();

            //act
            result.Should().Throw<ArgumentNullException>();
        }
    }
}
