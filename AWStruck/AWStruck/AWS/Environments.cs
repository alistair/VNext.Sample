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
            List<Filter> filters = GetEnvironmentTagFilters();
            DescribeTagsResponse result = GetTagsSync(ec2, filters);
            List<Environment> envs = result.Tags.GroupBy(
                x => x.Value,
                (s, results) => new Environment
                {
                    Name = s,
                    InstanceIds = results.Select(x => x.ResourceId).Distinct().ToList(),
                }).ToList();

            return envs.Select(env => EnchanceEnvProperties(ec2, env)).ToList();
        }

        private static List<Filter> GetEnvironmentTagFilters()
        {
            var filters = new List<Filter>
            {
                new Filter("key", new[] { "ENV" }.ToList()),
                new Filter("resource-type", new[] { "instance" }.ToList()),
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

        //public static string GetInstanceState(IAmazonEC2 ec2, string instanceId)
        //{
        //    //State is taken from the first instance
        //    DescribeInstancesResponse response = ec2.DescribeInstances(
        //        new DescribeInstancesRequest
        //        {
        //            InstanceIds = new List<string>
        //            {
        //                instanceId
        //            }
        //        });

        //    return response.DescribeInstancesResult.Reservations[0].Instances[0].State.Name.ToString();
        //}

        private static Environment EnchanceEnvProperties(IAmazonEC2 ec2, Environment environment)
        {
            DescribeInstancesResponse response = ec2.DescribeInstances(
                new DescribeInstancesRequest
                {
                    InstanceIds = new List<string>
                    {
                        environment.InstanceIds.FirstOrDefault()
                    }
                });

            environment.State = response.Reservations[0].Instances[0].State.Name.ToString();
            environment.Type = response.Reservations[0].Instances[0].InstanceType.ToString();

            return environment;
        }
    }
}
