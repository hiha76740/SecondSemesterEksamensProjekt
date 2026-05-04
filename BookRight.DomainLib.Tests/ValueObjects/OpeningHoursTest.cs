using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.ValueObjects
{
    public class OpeningHoursTest
    {
        private static DateTime Open => new DateTime(2030, 1, 1, 8, 0, 0);

        private static OpeningHours CreateWithValidData(DateTime open, DateTime close) => new OpeningHours(open, close);

        // ---------------------------------------------------------
        // 1. Create tests (Creating Opening Hours Value object)
        // ---------------------------------------------------------

        [Fact]
        public void Create_GivenCloseBeforeOpen_CastDomainException()
        {
            // Arrange
            var open = new DateTime(2026, 01, 01, 16, 0, 0);
            var close = new DateTime(2026,01,01,8,0,0);

            // Act & Assert
            Assert.Throws<DomainException>(() => CreateWithValidData(open, close));


        }

        [Fact]
        public void Create_GivenSameOpenAndClose_CastDomainException()
        {
            // Arrange
            var time = new DateTime(2026, 01, 01, 16, 0, 0);
            
            // Act & Assert
            Assert.Throws<DomainException>(() => CreateWithValidData(time, time));
        }

        [Fact]
        public void Create_GivenDifferentDay_CastDomainException()
        {
            // Arrange
            var close = Open.AddDays(1);

            // Act & Assert
            Assert.Throws<DomainException>(() => CreateWithValidData(Open, close));
        }
    }
}
