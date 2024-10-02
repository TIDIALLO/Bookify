
using Bookify.Domain.Abstractions;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Entities.Apartments;

public sealed class Apartment : Entity
{
    public Apartment(
        Guid id, 
        Address address,
        Name name, 
        Description description, 
        Money price, Money cleaningFee,
        List<Amenity> amenities) : base(id)
    {
        Address = address;
        Name = name;
        Description = description;
        Price = price;
        CleaningFee = cleaningFee;
        Amenities = amenities;
    }

    public Address Address { get; private set; }
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Money Price { get; private set; }
    public Money CleaningFee { get; private set; }
    public DateTime? LastBookedOnUtc { get; internal set; }
    public List<Amenity> Amenities { get; private set; } = new();

}
