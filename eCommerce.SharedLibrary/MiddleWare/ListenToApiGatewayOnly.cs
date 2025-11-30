using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eCommerce.SharedLibrary.MiddleWare
{
    public class ListenToApiGatewayOnly(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //extract specified header from the request
            var signedHeader = context.Request.Headers["Api-Gateway"];

            //NULL means, the request is not coming from the ApiGateway // 503 Status code
            if(signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, Service unavailable.");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
