using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
public class AddBookingValidator : AbstractValidator<BookingRequest>
{
    public AddBookingValidator()
    {
        // validate location
        RuleFor(x => x.Location).NotNull();
        RuleFor(x => x.Location.Address).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(6).MaximumLength(50);
        RuleFor(x => x.Location.Postcode).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(3).MaximumLength(8);
        RuleFor(x => x.Location.Latitude).NotEqual(0);
        RuleFor(x => x.Location.Longitude).NotEqual(0);
        RuleFor(x => x.Location.Details).MaximumLength(100);

        // validate schedule
        RuleFor(x => x.Schedule.StartDate).GreaterThan(DateOnly.FromDateTime(DateTime.Now));

        // validate user
        RuleFor(x => x.UserProfile.Id)
        .NotEmpty();

        // validate recycling items
        RuleFor(x => x.Recycling.NumberOfBags).GreaterThan(0);
        RuleForEach(x => x.Recycling.RecyclingItems).ChildRules(item =>
        {
            item.RuleFor(x => x.MaterialType).IsInEnum();
            item.RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        });
    }

}