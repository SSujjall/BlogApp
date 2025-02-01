using BlogApp.Application.Helpers.HelperModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogApp.Application.Helpers.ExtraHelpers
{
    public class ValidationHelper
    {
        public static ApiResponse<T> ValidateModelState<T>(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = new Dictionary<string, string>();
                foreach (var state in modelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors[state.Key] = error.ErrorMessage;
                    }
                }

                return ApiResponse<T>.Failed(errors, "Validation failed");
            }

            return null;
        }
    }
}
