using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MulitpleDb.Sample.Data
{
    public class GlobalCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        readonly IConfiguration _configuration;
        readonly ILogger<GlobalCommandInterceptor> _logger;
        readonly IWebHostEnvironment _hostEnvironment;
        public GlobalCommandInterceptor(
            IConfiguration configuration, 
            ILogger<GlobalCommandInterceptor> logger, 
            IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }
        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;

                if(_hostEnvironment.IsDevelopment())
                    _logger.LogInformation(@$"Intercepted command: {command.CommandText}");

                command.CommandText = command.CommandText.Replace(
                    "[Database2.dbo].",
                    "[Database2].[dbo].");

                if (_hostEnvironment.IsDevelopment())
                    _logger.LogInformation(@$"Intercepted command altered: {command.CommandText}");
            }
        }
    }
}
