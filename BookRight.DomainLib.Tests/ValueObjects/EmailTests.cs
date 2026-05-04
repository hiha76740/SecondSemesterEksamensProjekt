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

        [Fact]
        public void Create_WithInvalidEmail_CastDomainException()
        {
            // Arrange
            string email = "test@@mail.com";

            // Act & Assert
            Assert.Throws<DomainException>(() => CreateEmailWithValidData(email));
        }
    }
}
