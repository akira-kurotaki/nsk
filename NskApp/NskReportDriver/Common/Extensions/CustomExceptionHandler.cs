using Microsoft.AspNetCore.Diagnostics;

namespace NskReportDriver.Common.Extensions
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            this.logger = logger;
        }

        ValueTask<bool> IExceptionHandler.TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}", exceptionMessage, DateTime.UtcNow);
            // Return false to continue with the default behavior
            // - or - return true to signal that this exception is handled
            return ValueTask.FromResult(false);
        }
    }
}
