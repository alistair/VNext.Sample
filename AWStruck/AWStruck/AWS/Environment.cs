using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace AWStruck
{
	public class Environment
	{
		public Environment()
		{
			InstanceIds = new List<string>();
		}

		public string Name { get; set; }
		

        public int InstanceCount
        {
            get { return InstanceIds.Count(); }
        }

		public List<string> InstanceIds { get; set; }
    
    public bool IsAuto { get; set; }
    public CronDescription[] Descriptions { get; set; }
    public string State { get; set; }
        public string Type { get; set; }
        
     public Environment CloneWithAutoAndDescriptions(bool auto, CronDescription[] descs)
	  {
	    return new Environment()
	    {
	      Name = Name,
	      InstanceIds = InstanceIds,
	      IsAuto = auto,
        State = State,
        Descriptions = descs
	    };
	  }
        
	}

  public class CronDescription
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}