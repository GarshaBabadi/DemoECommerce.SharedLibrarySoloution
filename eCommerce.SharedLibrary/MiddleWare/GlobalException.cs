using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.MiddleWare
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Declare default variables
            string title = "error";
            string message = "Internal server error occured, please try again.";
            int status = (int)HttpStatusCode.InternalServerError;

            try
            {
                //Detecting the problerm HERE

                await next(context);

                //check if response is too many requests // 429 status code
                if(context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "waring";
                    message = "too many requests made.";
                    status = (int)StatusCodes.Status429TooManyRequests;   
                    await ModifyHeader(context, title, message, status);
                }

                //check if response is authorized // 401 status code
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are Unauthorized to access.";
                    status = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message, status);
                }

                //check if response is frobidden // 403 status code
                if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Alert";
                    message = "You are Forbidden / not required to access.";
                    status = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, status);
                }
            }
            catch(Exception ex)
            {
                //Log Original Exceptions to file, debugger or console
                LogException.LogExceptions(ex);

                //cheack if Exception is timed out // 408 Status code
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request time out... Please  try again.";
                    status = (int)StatusCodes.Status408RequestTimeout;
                }

                //if Exception caught or
                //if none of the exceptions happened then use the default variables
                await ModifyHeader(context, title, message, status);
            }
        }

        //Formatting the Response HERE
        private static async Task ModifyHeader(HttpContext context, string title, string message, int status)
        {
            //display scary-free message to the client
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Status = status,
                Detail = message,
            }), CancellationToken.None);
        }
    }
}
