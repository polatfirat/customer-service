using Customer.Domain.Exceptions;
using System.Net;
using System;
using System.Text.Json;

namespace Customer.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var errorResult = new ErrorResult
                {
                    Source = exception.TargetSite?.DeclaringType?.FullName,
                };

                switch (exception)
                {
                    case CustomerDatabaseException e:
                        errorResult.ErrorCode = (int)HttpStatusCode.InternalServerError;
                        errorResult.ErrorMessage = e.Message;
                        break;

                    case CustomerOperationException e:
                        errorResult.ErrorCode = (int)HttpStatusCode.Forbidden;
                        errorResult.ErrorMessage = e.Message;
                        break;
                    case CustomerValidationException e:
                        errorResult.ErrorCode = (int)HttpStatusCode.BadRequest;
                        errorResult.ErrorMessage = e.Message;
                        break;
                    default:
                        errorResult.ErrorCode = (int)HttpStatusCode.InternalServerError;
                        errorResult.ErrorMessage = exception.Message;
                        break;
                }

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = errorResult.ErrorCode;
                await response.WriteAsync(JsonSerializer.Serialize(errorResult));
            }
        }
    }
}
