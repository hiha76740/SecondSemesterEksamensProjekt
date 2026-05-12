using BookRight.DomainLib.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ComplexProperty(
            c => c.Address,
            a => a.ToJson());

        builder.ComplexProperty(
            c => c.Email,
            e => e.ToJson());

        builder.ComplexProperty(
            c => c.PhoneNumber,
            p => p.ToJson());
    }
}
