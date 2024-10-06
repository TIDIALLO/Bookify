using Bookify.Domain.Abstractions;
using Bookify.Domain.Entities.Apartments;
using Bookify.Domain.Entities.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Entities.Bookings;

public sealed class Booking : Entity
{
    private Booking(
        Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money cleaningFee,
        Money amenitiesUpcharge,
        Money totalPrice,
        Money totalPrice1,
        BookingStatus status,
        DateTime createdOnUtc
    ) : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        CleaningFee = cleaningFee;
        AmenitiesUpcharge = amenitiesUpcharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;

    }

    #region variables
    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpcharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public Money PriceForPeriod { get; private set; }

    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CanceledOnUtc { get; private set; }
    #endregion

    public static Booking Reserve(
        Apartment apartment,
        Guid userId,
        DateRange duration, 
        DateTime utcNow,
        PricingService pricingService
    )
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        var booking = new Booking(
            Guid.NewGuid(),
            apartment.Id,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpcharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow
        );

        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));
        apartment.LastBookedOnUtc = utcNow;

        return booking;
    }

    public Result Confirm(DateTime UtcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = UtcNow;

        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }


    public Result Reject(DateTime UtcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }

        Status = BookingStatus.Rejected;
        RejectedOnUtc = UtcNow;

        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }

    public Result Complete(DateTime UtcNow)
    {
        if (Status != BookingStatus.Completed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }

        Status = BookingStatus.Completed;
        RejectedOnUtc = UtcNow;

        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

        return Result.Success();
    }

    public Result Cancel(DateTime UtcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }
        var currentDate = DateOnly.FromDateTime(UtcNow);
        if(currentDate > Duration.Start)
        {
            return Result.Failure(BookingErrors.AlreadyStarted);
        }

        Status = BookingStatus.Cancelled;
        RejectedOnUtc = UtcNow;

        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }
}


