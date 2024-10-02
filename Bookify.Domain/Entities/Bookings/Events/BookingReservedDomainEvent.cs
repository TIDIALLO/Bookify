using Bookify.Domain.Abstractions;
using Bookify.Domain.Entities.Bookings;

namespace Bookify.Domain.Entities.Bookings.Events
{
    public record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;

}
