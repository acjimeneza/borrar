using Deluxe.TokenRefresh.Domain;
using System.Text;

namespace Deluxe.TokenRefresh.Infrastructure.Deluxe
{
    public static class ErrorService
    {
        public static string BuildError(ErrorResponse? errorResponse)
        {
            if (errorResponse == null)
            {
                return string.Empty;
            }

            var message = new StringBuilder();

            message.AppendLine($"Error: {errorResponse.Error}");

            if (errorResponse.Errors != null && errorResponse.Errors.Any())
            {
                message.AppendLine($"Errors:");
                foreach (var error in errorResponse.Errors)
                {
                    message.AppendLine($"- {error}");
                }
            }

            if (errorResponse.ErrorData != null && errorResponse.ErrorData.Any())
            {
                message.AppendLine("ErrorData");
                foreach (var error in errorResponse.ErrorData)
                {
                    message.AppendLine(error.Key);
                    foreach(var value in error.Value)
                    {
                        message.AppendLine(value);
                    }
                }
            }

            return message.ToString();
        }
    }
}
