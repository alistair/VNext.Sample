using System.Linq;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace AWStruck.AWS
{
	public static class Instances
	{
		public static void Stop(IAmazonEC2 ec2, params string[] ids)
		{
			ec2.StopInstances(new StopInstancesRequest(ids.ToList()));
		}

		public static void Start(IAmazonEC2 ec2, params string[] ids)
		{
			ec2.StartInstances(new StartInstancesRequest(ids.ToList()));
		} 
	}
}