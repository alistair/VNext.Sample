using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Amazon;
using Amazon.EC2;
using Hangfire;

namespace AWStruck.AWS
{
	public static class Tasks
	{

		public static void StopInstancesTask(string taskId)
		{
			//StopInstances(CreateAmazonClient(), "i-10f40d1e");
			Console.WriteLine("StopInstance " + taskId);
			BackgroundJob.Schedule(() => StartInstancesTask(taskId), DateTimeOffset.UtcNow.AddMinutes(5));
		}

		public static void StartInstancesTask(string taskId)
		{
			DoWorkAndBecome(() => Instances.Start(Global.CreateAmazonClient()),
				() => BackgroundJob.Schedule(() => StopInstancesTask(taskId), DateTimeOffset.UtcNow.AddMinutes(5)));

			//StopInstances(CreateAmazonClient(), "i-10f40d1e");
			//Console.WriteLine("StartInstance " + taskId);
			//BackgroundJob.Schedule(() => StopInstancesTask(taskId), DateTimeOffset.UtcNow.AddMinutes(5));
		}

		public static void DoWorkAndBecome(Action work, Action reschedule)
		{
			work();
			reschedule();
		}

		public static void Schedule(Expression<Action> action, DateTimeOffset scheduledTime)
		{
			BackgroundJob.Schedule(action, scheduledTime);
		}

		
	}
}