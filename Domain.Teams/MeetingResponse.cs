using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Teams
{
    public class MeetingResponse
    {
        public DateTime creationDateTime { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string id { get; set; }
        public string joinWebUrl { get; set; }
        public string subject { get; set; }
        
    }
}
