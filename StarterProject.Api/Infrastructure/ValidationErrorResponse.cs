using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Infrastructure
{
    public class ValidationErrorResponse
    {
        public List<ValidationError> ValidationErrors { get; set; }
    }

    public class ValidationError
    {
        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }
    }

    public static class ValidationHelpers
    {
        public static ValidationErrorResponse ConvertValidationResult(ValidationResult validationResult)
        {
            var validationErrorResponse = new ValidationErrorResponse();
            validationErrorResponse.ValidationErrors = new List<ValidationError>();
            foreach (var item in validationResult.Errors)
            {
                var validationError = new ValidationError
                {
                    ErrorMessage = item.ErrorMessage,
                    PropertyName = item.PropertyName
                };

                validationErrorResponse.ValidationErrors.Add(validationError);
            }

            return validationErrorResponse;
        }
    }
}
