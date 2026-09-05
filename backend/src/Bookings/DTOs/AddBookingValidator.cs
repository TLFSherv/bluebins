using FluentValidation;
public class AddBookingValidator : AbstractValidator<BookingDTO>
{
    public AddBookingValidator()
    {
        RuleFor(x => x.Status).IsInEnum().Equals(BookingStatus.Draft);
        RuleFor(x => x.CollectionDate).GreaterThan(DateTime.Now);

        // validate location
        RuleFor(x => x.Location).NotNull();
        RuleFor(x => x.Location.AddressLine1).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(6).MaximumLength(50);
        RuleFor(x => x.Location.Postcode).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(3).MaximumLength(8);
        RuleFor(x => x.Location.Latitude).NotEqual(0);
        RuleFor(x => x.Location.Longitude).NotEqual(0);
        RuleFor(x => x.Location.Details).MaximumLength(100);

        // validate schedule

        // validate recycling items
        RuleFor(x => x.RecyclingItems).NotNull();
        RuleForEach(x => x.RecyclingItems).ChildRules(item =>
        {
            item.RuleFor(x => x.MaterialType).IsInEnum();
            item.RuleFor(x => x.ItemCount).GreaterThan(0);
        });
    }

}