using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dotnetstarter.organisations.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var port = Environment.GetEnvironmentVariable("PORT");
            port = String.IsNullOrEmpty(port) ? "5001" : port;
            //debugging statement in case the port was not passed correctly
            Console.WriteLine($"env PORT is {port ?? ("not found")}");
            Console.WriteLine($"TENANT is {Environment.GetEnvironmentVariable("TENANT")}");

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureKestrel((context, options) =>
                {
                    options.Listen(IPAddress.IPv6Any, Convert.ToInt32(port));
                });
        }
    }
}
