using Application.DTOs.City;
using FluentValidation;

namespace Application.Validations;

public class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(10).WithMessage("Code cannot exceed 10 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");
    }
}