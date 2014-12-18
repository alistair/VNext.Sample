using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.EC2;
using Antlr.Runtime.Misc;

namespace AWStruck.AWS
{
	public static class Global
	{
		private static IEnumerable<Environment> GetEnvironments()
		{
			return Environments.GetEnvironments(CreateAmazonClient());
		}

		private static void EnvironmentUp(string taskId)
		{
			Tasks.DoWorkAndBecome(
				() => Environments.StartEnvironment(taskId),
				() => Tasks.Schedule(Environments.StopEnvironment(taskId), new DateTimeOffset().AddMinutes(5)));
		}

		private static void EnvironmentDown(string taskId)
		{
			Tasks.DoWorkAndBecome(
				() => Environments.StopEnvironment(taskId),
				() => Tasks.Schedule(Environments.StartEnvironment(taskId), new DateTimeOffset().AddMinutes(5)));
		}

		public static IAmazonEC2 CreateAmazonClient()
		{
			var amazonEC2Config = new AmazonEC2Config
			{
				RegionEndpoint = RegionEndpoint.USWest2
			};
			return new AmazonEC2Client(amazonEC2Config);
		}
	}
}
