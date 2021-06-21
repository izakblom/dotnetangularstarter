using Common.Exceptions;
using Common.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace dotnetstarter.gateway.api.Application.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private ICustomLogger _customLogger;

        public ErrorHandlingMiddleware(RequestDelegate next, ICustomLogger customLogger)
        {
            this.next = next;
            _customLogger = customLogger ?? throw new ArgumentNullException(nameof(customLogger));
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {


            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is UserNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (exception is GeneralDomainException) code = HttpStatusCode.BadRequest;
            else if (exception is UserDisabledException) code = HttpStatusCode.Unauthorized;

            string errorCode = null;
            if (exception is UserDisabledException) errorCode = "user/disabled";

            var result = JsonConvert.SerializeObject(new { error = exception.Message, errorCode = errorCode });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                //Log
                var labels = new Dictionary<string, string>();
                labels.Add("source", ex.Source);
                labels.Add("context", "gateway");
                _customLogger.LogMessage(LoggerSeverity.Error, ex.ToString(), ex.StackTrace, labels);

                await HandleExceptionAsync(context, ex);
            }
        }
    }
}
