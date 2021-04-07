using _09Game.NewActivity.Core.Log;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace _09Game.NewActivity.Core.Util
{
    public static class PerformanceLogExtension
    {
        public static IApplicationBuilder UsePerformanceLog(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                var Request = context.Request.Path.ToString();
                VisitLog.Visit(Request);
                var sw = new Stopwatch();
                sw.Start();
                await next();
                sw.Stop();
                if (sw.ElapsedMilliseconds > 3000)
                    Logger.Info($"TraceId:{context.TraceIdentifier}, RequestMethod:{context.Request.Method}, RequestPath:{context.Request.Path + context.Request.QueryString}, ElapsedMilliseconds:{sw.ElapsedMilliseconds}, Response StatusCode: {context.Response.StatusCode}");
            });
            return applicationBuilder;
        }
    }
}
