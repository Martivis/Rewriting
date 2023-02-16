using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders;

public class AddResultModel
{
    public Guid ContractUid { get; set; }
    public string Text { get; set; }
}

public class AddResultModelValidator : AbstractValidator<AddResultModel>
{
    public AddResultModelValidator() 
    {
        RuleFor(t => t.ContractUid).NotEmpty().WithMessage("ContractUid is required");
        RuleFor(t => t.Text).NotEmpty().WithMessage("Text is required");
    }
}
