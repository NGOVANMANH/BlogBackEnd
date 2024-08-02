using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Utils
{
    public static class ModelStateUtil
    {
        public static Dictionary<string, string> FormatModelStateErrors(ModelStateDictionary modelState)
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
}
