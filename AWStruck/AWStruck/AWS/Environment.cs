using System.Collections.Generic;
using System.Linq;

namespace AWStruck
{
	public class Environment
	{
		public Environment()
		{
			InstanceIds = new List<string>();
		}

		public string Name { get; set; }
		public int InstanceCount { get { return InstanceIds.Count(); } }
		public List<string> InstanceIds { get; set; }
    public string State { get; set; }
	}
}
