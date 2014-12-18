using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.OpsWorks.Model;
using Amazon.SimpleWorkflow.Model;
using Hangfire;

namespace AWStruck
{
	public static class AWSTasks
	{
		public static Expression<Action> Test(string something)
		{
			return () => Console.WriteLine("fdfsdfs");
		}

		public static void StopInstancesTask(string taskId)
		{
			//StopInstances(CreateAmazonClient(), "i-10f40d1e");
			Console.WriteLine("StopInstance " + taskId);
			BackgroundJob.Schedule(() => StartInstancesTask(taskId), DateTimeOffset.UtcNow.AddMinutes(5));
		}

		public static void StartInstancesTask(string taskId)
		{
			DoWork(() => StartInstances(CreateAmazonClient()), 
				() => BackgroundJob.Schedule(() => StopInstancesTask(taskId), DateTimeOffset.UtcNow.AddMinutes(5)));

			//StopInstances(CreateAmazonClient(), "i-10f40d1e");
			//Console.WriteLine("StartInstance " + taskId);
			//BackgroundJob.Schedule(() => StopInstancesTask(taskId), DateTimeOffset.UtcNow.AddMinutes(5));
		}

		public static void DoWork(Action work, Action reschedule)
		{
			work();
			reschedule();
		}

		public static void Schedule(Expression<Action> action, DateTimeOffset scheduledTime)
		{
			BackgroundJob.Schedule(action, scheduledTime);
		}

		static IAmazonEC2 CreateAmazonClient()
		{
			var amazonEC2Config = new AmazonEC2Config
			{
				RegionEndpoint = RegionEndpoint.USWest2
			};
			return new AmazonEC2Client(amazonEC2Config);
		}

		static void StopInstances(IAmazonEC2 ec2, params string[] ids)
		{
			ec2.StopInstances(new StopInstancesRequest(ids.ToList()));
		}

		static void StartInstances(IAmazonEC2 ec2, params string[] ids)
		{
			ec2.StartInstances(new StartInstancesRequest(ids.ToList()));
		}

		static Task<IEnumerable<Environment>> GetEnvironments()
		{
			return GetEnvironments(CreateAmazonClient());
		}

		private static async Task<IEnumerable<Environment>> GetEnvironments(IAmazonEC2 ec2)
		{
			var filters = GetEnvironmentTagFilters();
			var result = await GetTags(ec2, filters);
			return result.Tags.GroupBy(x => x.Value, (s, results) =>
			{
				return new Environment
				{
					Name = s,
					InstanceIds = results.Select(x => x.ResourceId).Distinct().ToList()
				};
			});
		}

		private static List<Filter> GetEnvironmentTagFilters()
		{
			var filters = new List<Filter>
			{
				new Filter("key", new[] {"ENV"}.ToList()),
				new Filter("resource-type", new[] {"instance"}.ToList()),
			};
			return filters;
		}

		private static Task<DescribeTagsResponse> GetTags(IAmazonEC2 ec2, List<Filter> filters)
		{
			return ec2.DescribeTagsAsync(new DescribeTagsRequest(filters));
		}
	}

	public class Environment
	{
		public Environment()
		{
			InstanceIds = new List<string>();
		}

		public string Name { get; set; }
		public int InstanceCount { get { InstanceIds.Count(); } }
		public List<string> InstanceIds { get; set; }
	}
}