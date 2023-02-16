using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public class AddOfferModel
{
    public Guid OrderUid { get; set; }
    public Guid ContractorUid { get; set; }
    public decimal Price { get; set; }
    public string Comment { get; set; }
}

public class AddOfferModelValidator : AbstractValidator<AddOfferModel>
{
    public AddOfferModelValidator()
    {
        RuleFor(t => t.OrderUid).NotEmpty().WithMessage("OrderUid is required");
        RuleFor(t => t.ContractorUid).NotEmpty().WithMessage("ContractorUid is required");
        RuleFor(t => t.Price).GreaterThan(0).WithMessage("Price should be positive");
        RuleFor(t => t.Comment).NotEmpty().WithMessage("Comment is required");
    }
}
