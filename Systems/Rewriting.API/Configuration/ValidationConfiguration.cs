using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Responses;
using Rewriting.Common.Validator;

namespace Rewriting.API.Configuration
{
    public static class ValidationConfiguration
    {
        public static IMvcBuilder AddValidator(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var fieldErrors = new List<ErrorResponseFieldInfo>();
                    foreach (var (field, state) in context.ModelState)
                    {
                        if (state.ValidationState == ModelValidationState.Invalid)
                            fieldErrors.Add(new ErrorResponseFieldInfo()
                            {
                                FieldName = field,
                                Message = string.Join(", ", state.Errors.Select(x => x.ErrorMessage))
                            });
                    }

                    var result = new BadRequestObjectResult(new ErrorResponse()
                    {
                        ErrorCode = 100,
                        Message = "One or more validation errors occurred.",
                        FieldErrors = fieldErrors
                    });

                    return result;
                };
            });

            var services = builder.Services;

            services.AddFluentValidationAutoValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
            });

            var validators = from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                             where !type.IsAbstract && !type.IsGenericTypeDefinition
                             let interfaces = type.GetInterfaces()
                             let genericInterfaces = interfaces.Where(i =>
                                 i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
                             let matchingInterface = genericInterfaces.FirstOrDefault()
                             where matchingInterface != null
                             select new
                             {
                                 InterfaceType = matchingInterface,
                                 ValidatorType = type
                             };

            validators.ToList().ForEach(x => { services.AddSingleton(x.InterfaceType, x.ValidatorType); });

            services.AddSingleton(typeof(IModelValidator<>), typeof(ModelValidator<>));

            return builder;
        }
    }
}
