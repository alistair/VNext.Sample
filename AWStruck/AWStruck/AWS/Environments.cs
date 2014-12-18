using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace AWStruck.AWS
{
	public static class Environments
	{
		public static IEnumerable<Environment> GetEnvironments(IAmazonEC2 ec2)
		{
			var filters = GetEnvironmentTagFilters();
      var result = GetTagsSync(ec2, filters);
			return result.Tags.GroupBy(x => x.Value, (s, results) => new Environment
			{
				Name = s,
				InstanceIds = results.Select(x => x.ResourceId).Distinct().ToList()
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

    private static DescribeTagsResponse GetTagsSync(IAmazonEC2 ec2, List<Filter> filters)
    {
      return ec2.DescribeTags(new DescribeTagsRequest(filters));
    }

	  public static void StartEnvironmentInternal(string taskId)
	  {
      Instances.Start(Global.CreateAmazonClient(), GetEnvironments(Global.CreateAmazonClient()).First(x => x.Name == taskId).InstanceIds.ToArray());
	  }

		public static Expression<Action> StartEnvironment(string taskId)
		{
		  return () => StartEnvironmentInternal(taskId);
		}

    public static void StopEnvironmentInternal(string taskId)
    {
      Instances.Stop(Global.CreateAmazonClient(), GetEnvironments(Global.CreateAmazonClient()).First(x => x.Name == taskId).InstanceIds.ToArray());
    }

		public static Expression<Action> StopEnvironment(string taskId)
		{
		  return () => StopEnvironmentInternal(taskId);
		}
	}
}
