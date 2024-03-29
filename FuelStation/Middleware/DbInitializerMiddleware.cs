﻿using OrganizationsWaterSupply.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationsWaterSupply.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public Task Invoke(HttpContext context, OrganizationsWaterSupplyContext db)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                // инициализация базы данных
                DbInitializer.Initialize(db);
                context.Session.SetString("starting", "Yes");
            }

            // Call the next delegate/middleware in the pipeline
            return _next.Invoke(context);
        }
    }
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }

    }

}
