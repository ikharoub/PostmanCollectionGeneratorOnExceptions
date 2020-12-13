using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PostmanCollectionGeneratorOnExceptions.Middleware.PostmanHelper;

namespace PostmanCollectionGeneratorOnExceptions.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // enable multiple readings for request body
                context.Request.EnableBuffering();

                // invoke the next middleware 
                await _next(context);
            }
            catch (Exception e)
            {
                // clear the response and logs the exceptions
                LogException(context, e);
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        // asynchronously write into post man collection     
        private static void LogException(HttpContext context, Exception e) => CollectionWriter.WriteIntoCollection(context, e);
    }
}
