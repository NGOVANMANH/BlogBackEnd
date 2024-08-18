using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Extensions.ModelState;

public static class ModelStateExtensions
{
    public static Dictionary<string, string> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = new Dictionary<string, string>();

        foreach (var state in modelState)
        {
            if (state.Value.Errors.Count > 0)
            {
                errors[state.Key] = state.Value.Errors.First().ErrorMessage;
            }
        }

        return errors;
    }
}