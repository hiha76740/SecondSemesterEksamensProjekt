using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using System.Net;
using Xunit;

namespace BookRight.DomainLib.Tests;

public class CustomerTests
{
    private static string Firstname => "Poul";
    private static string Lastname => "Pedersen";
    private static DateTime Birthdate => new DateTime(1965, 6, 18);
    private static string Note => "Peanutbutter-allergy";
    private static Address Address => new Address("Test Avenue 21", "1234", "Testville");
    private static Email Email => new Email("PoulP@testmail.com");
    private static PhoneNumber PhoneNumber => new PhoneNumber("87654321");

    private static Customer CreateCustomerWithValidData(
        string? firstName = null,
        string? lastName = null,
        DateTime? birthDate = null,
        Address? address = null,
        Email? email = null,
        PhoneNumber? phoneNumber = null,
        Guid? therapistId = null)
        => Customer.Create(
            firstName ?? Firstname,
            lastName ?? Lastname,
            birthDate ?? Birthdate,
            address ?? Address,
            email ?? Email,
            phoneNumber ?? PhoneNumber,
            Note,
            therapistId);


    //Arrange

    //Act

    //Assert
}
