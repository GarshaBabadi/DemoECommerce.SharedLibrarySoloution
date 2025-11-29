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
        }
    }
}
