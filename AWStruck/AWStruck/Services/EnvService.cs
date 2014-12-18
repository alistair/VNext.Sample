using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using AWStruck.AWS;

namespace AWStruck.Services
{
  public class EnvService : IEnv
  {
    private readonly AmazonEC2Config _amazonEC2Config;
    private readonly List<string> _ids;
    private readonly IAmazonEC2 _ec2;

    public EnvService()
    {
      _amazonEC2Config = new AmazonEC2Config()
      {
        RegionEndpoint = RegionEndpoint.USWest2
      };

      _ids = new List<string>() { "i-10f40d1e" };

      _ec2 = AWSClientFactory.CreateAmazonEC2Client(_amazonEC2Config);
    }

    public StartInstancesResponse Start()
    {
      return _ec2.StartInstances(new StartInstancesRequest(_ids));
    }

    public StopInstancesResponse Stop()
    {
      return _ec2.StopInstances(new StopInstancesRequest(_ids));
    }

    public IEnumerable<Environment> Envs()
    {
      return Environments.GetEnvironments(_ec2);
    }
  }
}
