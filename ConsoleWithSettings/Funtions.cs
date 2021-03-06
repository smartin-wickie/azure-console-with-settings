using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ConsoleWithSettings
{
	public class Funtions
	{
		public static void ProcessQueueMessage([QueueTrigger("do-the-thing")] string message, ILogger logger)
		{
			Thread.Sleep(30000);
			logger.LogWarning($"ProcessQueueMessage {DateTime.UtcNow.ToString("s")} {message}" );
			Thread.Sleep(30000);
		}
	}
}
