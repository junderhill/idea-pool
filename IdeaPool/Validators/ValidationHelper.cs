using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyIdeaPool.Validators
{
    public static class ValidationHelper
    {
        public static ModelStateDictionary ToModelStateDictionary(this ValidationResult result)
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var e in result.Errors)
            {
                modelStateDictionary.AddModelError(e.PropertyName, e.ErrorMessage);
            }

            return modelStateDictionary;
        } 
    }
}