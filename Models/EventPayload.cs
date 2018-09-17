using System;
using Newtonsoft.Json.Linq;

namespace DIS.Models
{
    public class EventPayload
    {


        public Context context { get; set; }
        public Info info { get; set; }

        public class Context
        {
            public string domain { get; set; }
            public string application { get; set; }
            public string serviceName { get; set; }
            public string eventVersion { get; set; }
            public System.DateTime eventGeneratedAtTime { get; set; }
            public string correlationID { get; set; }
            public string requestID { get; set; }

        }

        public class Info
        {
            public string filename { get; set; }
            public string source { get; set; }
            public string apipath { get; set; }

        }

        public EventPayload()
        {
            context = new Context();
            info = new Info();
        }
    }

    public class NotFound{

        public string FileName { get; set; }
        public System.DateTime RequestedAt { get; set; }
        public string Error { get; set; }
    }
}
