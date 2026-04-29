using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using System.Net;
using Xunit;

namespace BookRight.DomainLib.Tests;

public class CustomerTests
{
    private static string FirstName => "Poul";
    private static string LastName => "Pedersen";
    private static DateTime Birthdate => new DateTime(1965, 6, 18);
    private static string? Note => "Peanutbutter-allergy";
    private static Address Address => new Address("Test Avenue 21", "1234", "Testville");
    private static Email Email => new Email("PoulP@testmail.k");
    private static PhoneNumber PhoneNumber => new PhoneNumber("87654321");


    //Arrange

    //Act

    //Assert
}
