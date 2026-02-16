using Application.DTOs.PropertyAd;
using FluentValidation;

namespace Application.Validations;

public class CreatePropertyAdRequestValidator : AbstractValidator<CreatePropertyAdRequest>
{
    public CreatePropertyAdRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(300).WithMessage("Location cannot exceed 300 characters");

        RuleFor(x => x.RoomCount)
            .InclusiveBetween(1, 50).WithMessage("Room count must be between 1 and 50");

        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("Area must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Area cannot exceed 10000 sqm");

        RuleFor(x => x.OfferType)
            .IsInEnum().WithMessage("Invalid offer type");

        RuleFor(x => x.RealEstateType)
            .IsInEnum().WithMessage("Invalid real estate type");
    }
}