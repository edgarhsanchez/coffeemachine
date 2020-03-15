using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Maker
{
    /// <summary>
    /// Middleware to trigger an empty log entry to be picked up by NLog
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        /// <summary>
        /// The next handler
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// The request logger
        /// </summary>
        private readonly ILogger _requestLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _requestLogger = loggerFactory.CreateLogger("HttpRequestLog");
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            //code dealing with the request

            await _next(context);

            // layout renderers will pick up the details
            _requestLogger.LogInformation(" ");
        }


    }

    /// <summary>
    /// Middleware extension method to make it easier to install the middleware into the pipeline
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Uses the request logging.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IApplicationBuilder.</returns>
        /// TODO Edit XML Comment Template for UseRequestLogging
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}