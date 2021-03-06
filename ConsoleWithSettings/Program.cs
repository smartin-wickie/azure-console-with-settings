using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ConsoleWithSettings
{
	class Program
	{
		static async Task Main(string[] args)
		{

			Console.WriteLine("Hello World!");

			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
				.AddEnvironmentVariables()
				;
			var config = builder.Build();
			var appInsightsInstrumentationKey = config.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");
			var minimumLoggingLevel = config.GetValue<int>("CONSOLE_TEST_MINIMUM_LOG_LEVEL")
					== 0 ? LogLevel.Information : (LogLevel)config.GetValue<int>("CONSOLE_TEST_MINIMUM_LOG_LEVEL");

			var hostBuilder = Host
				.CreateDefaultBuilder(args)
				.ConfigureWebJobs(webJobsBuilder =>
				{
					
					webJobsBuilder.AddAzureStorageCoreServices();
					webJobsBuilder.AddAzureStorage(queueOptions =>
					{
						queueOptions.BatchSize = 4;
						queueOptions.NewBatchThreshold = 2;
						queueOptions.MaxDequeueCount = 2;
						queueOptions.MaxPollingInterval = TimeSpan.FromSeconds(30);

					}); 
				})
				.ConfigureLogging(logging =>
				{
					//logging.AddConsole(); //Added by Default unless you clear
					if (appInsightsInstrumentationKey != null)
					{
						logging.AddConsole();
						logging.AddApplicationInsights(appInsightsInstrumentationKey, options =>
						{
							options.FlushOnDispose = true;
						});
					}
					logging.SetMinimumLevel(minimumLoggingLevel);
				}).ConfigureServices((hostContext, services) =>
				{
					//DI Stuff HERE	
				})
				
				;

			var host = hostBuilder.Build();
			using (host)
			{
				var logger = host.Services.GetRequiredService<ILogger<Program>>();
				logger.LogInformation("We Have a Loggger!!");

				var someEnvVar = config.GetValue<string>("customVar");
				logger.LogWarning($"custom var from environment {someEnvVar}");

				var path = config.GetValue<string>("Path");
				logger.LogInformation($"path var from environment {path}");

				var conString = config.GetValue<string>("ConnectionStrings:ShopperContext");
				logger.LogInformation($"Connection string from GetVal Path {conString}");

				var conAsConString = config.GetConnectionString("ShopperContext");
				logger.LogInformation($"Connection string from GetConnectionString {conAsConString}");

				var db = config.GetSection("ConnectionStrings");
				var conFromSection = db.GetValue<string>("ShopperContext");
				logger.LogInformation($"Connection string from Section GetValue {conFromSection}");
				logger.LogDebug($"Connection string from Section GetValue {conFromSection}");


				var escapia = new EscapiaSettings();
				config.GetSection("Ins.Shopper.Proxy.Escapia").Bind(escapia);

				logger.LogWarning(JsonConvert.SerializeObject(escapia));
				logger.LogError(JsonConvert.SerializeObject(escapia));
				
				logger.LogError("This is an Error ",
					new ApplicationException("Application Error",
						new ApplicationException("Inner Exception")));
				logger.LogCritical("This is a CRITICAL Error ",
					new ApplicationException("CRITICAL Error",
						new ApplicationException("Inner Exception")));

				await host.RunAsync();
			}
		}
	}
}
