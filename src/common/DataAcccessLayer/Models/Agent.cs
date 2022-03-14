using DataAcccessLayer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcccessLayer.Models
{
    public class Agent
    {
        public string Id { get; set; }
        public string SeniortyLevel { get; set; }
        public string Shift { get; set; }
        internal bool Active { get; set; }

        internal bool FromOverflow { get; set; } 
        public float SeniortyPoint => this.SeniortyLevel.Equals(SeniorityLevels.JUNIOR) ? 0.4f : (
            this.SeniortyLevel.Equals(SeniorityLevels.MID_LEVEL) ? 0.6f : (
                this.SeniortyLevel.Equals(SeniorityLevels.SENIOR) ? 0.8f : 0.5f) );
         
        public List<string> ActiveSessionId { get; set; } = new List<string>();
         
        //TODO
        //trigger active session check in Active session queue if try/when try to signout
    }
}
