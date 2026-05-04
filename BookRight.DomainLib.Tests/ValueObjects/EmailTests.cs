using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.ValueObjects
{
    public class EmailTests
    {
        private static string EmailAddress => "john@doe.com";

        private static Email CreateEmailWithValidData(string? emailAddress = null) => new Email(emailAddress ?? EmailAddress);


        // ---------------------------------------------------------
        // 1. Create tests (Creating an Email)
        // ---------------------------------------------------------

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_GivenEmptyEmail_CastDomainException(string email)
        {
            // Act & Assert
            Assert.Throws<DomainException>(() => CreateEmailWithValidData(email));
        }
    }
}
