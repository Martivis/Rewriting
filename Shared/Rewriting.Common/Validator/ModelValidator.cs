using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Common.Validator;

public class ModelValidator<T> : IModelValidator<T> where T : class
{
    private readonly IValidator<T> _validator;

    public ModelValidator(IValidator<T> validator)
    {
        _validator = validator;
    }

    public void Check(T model)
    {
        var result = _validator.Validate(model);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
