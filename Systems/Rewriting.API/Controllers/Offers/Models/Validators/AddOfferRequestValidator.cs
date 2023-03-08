using FluentValidation;

namespace Rewriting.API.Controllers.Offers.Models.Validators
{
    public class AddOfferRequestValidator : AbstractValidator<AddOfferRequest>
    {
        public AddOfferRequestValidator()
        {
            RuleFor(t => t.OrderUid).NotEmpty().WithMessage("OrderUid is required");
            RuleFor(t => t.Price).GreaterThan(0).WithMessage("Price should be positive");
            RuleFor(t => t.Comment).NotEmpty().WithMessage("Comment is required");
        }
    }
}
