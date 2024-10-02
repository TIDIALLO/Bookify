using Bookify.Domain.Entities.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Entities.Bookings
{
    public class PricingService
    {
        public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
        {
            var currency = apartment.Price.Currency;

            var priceForPeriod = new Money(
                apartment.Price.Amount * period.LengthInDays,
                currency);

            decimal percentageUpCharge = 0;
            foreach(var amenity in apartment.Amenities)
            {
                percentageUpCharge += amenity switch
                {
                    Amenity.GardenView or Amenity.MontainView => 0.5m,
                    Amenity.AirConditioning => 0.01m,
                    Amenity.Parking => 0.01m,
                    _ => 0,
                };
            }

            var amenytiesUpCharge = Money.Zero(currency);
            if (percentageUpCharge > 0)
            {
                amenytiesUpCharge = new Money(
                    priceForPeriod.Amount * percentageUpCharge,
                    currency
                );
            }

            var totalPrice = Money.Zero();
            totalPrice += priceForPeriod;

            if (!apartment.CleaningFee.IzZero())
            {
                totalPrice += apartment.CleaningFee;
            }
            totalPrice += amenytiesUpCharge;

            return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenytiesUpCharge, totalPrice);
        }
    }
}
