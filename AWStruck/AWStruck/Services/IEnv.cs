using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;

namespace AWStruck.Services
{
  public interface IEnv
  {
    StartInstancesResponse Start();
    StopInstancesResponse Stop();
  }
}
