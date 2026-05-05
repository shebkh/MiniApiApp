using EventsApi.DTOs;
using FluentValidation;

public class CreateEventValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(150);

        RuleFor(e => e.Description)
            .MaximumLength(500)
            .When(e => e.Description != null);

        RuleFor(e => e.Date)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Event date must be in the future");

        RuleFor(e => e.Location)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(e => e.OrganizerId)
            .GreaterThan(0).WithMessage("Valid organizer required");
    }
}

public class CreateOrganizerValidator : AbstractValidator<CreateOrganizerDto>
{
    public CreateOrganizerValidator()
    {
        RuleFor(o => o.Name).NotEmpty().MaximumLength(100);
        RuleFor(o => o.Email).NotEmpty().EmailAddress()
            .WithMessage("A valid email is required");
        RuleFor(o => o.Phone).MaximumLength(20)
            .When(o => o.Phone != null);
    }
}

public class CreateTicketValidator : AbstractValidator<CreateTicketDto>
{
    public CreateTicketValidator()
    {
        RuleFor(t => t.Type).NotEmpty().MaximumLength(50);
        RuleFor(t => t.Price).GreaterThan(0)
            .WithMessage("Price must be positive");
        RuleFor(t => t.QuantityAvailable).GreaterThanOrEqualTo(0);
    }
}