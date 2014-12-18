using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace AWStruck.Services
{
  public class EnvService
  {
    public void Start()
    {
      var amazonEC2Config = new AmazonEC2Config
      {
        RegionEndpoint = RegionEndpoint.USWest2
      };

      IAmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client(amazonEC2Config);
      var ids = new List<string>() { "i-10f40d1e" };
      var response = ec2.StartInstances(new StartInstancesRequest(ids));
      Console.WriteLine(response.ToString());
    }

    public void Stop()
    {
      var amazonEC2Config = new AmazonEC2Config
      {
        RegionEndpoint = RegionEndpoint.USWest2
      };

      IAmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client(amazonEC2Config);
      var ids = new List<string>() { "i-10f40d1e" };
      var response = ec2.StopInstances(new StopInstancesRequest(ids));
      Console.WriteLine(response.ToString());
    }
  }
}
