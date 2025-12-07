using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config, string fileName) where TContext: DbContext
        {
            //Add Generic Database context
            services.AddDbContext<TContext>(option =>
            option.UseSqlServer(config.GetConnectionString("eCommerceConnection"),
            sqlserverOption => sqlserverOption.EnableRetryOnFailure()));
           
            // congigure serilog logging
            return services;
        }
    }
}
