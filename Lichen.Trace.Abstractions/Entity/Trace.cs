using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Trace.Abstractions.Entity
{
    public class TraceEntity
    {
        public Guid? Id { get; set; }
        public string LogString { get; set; }
        public object LogObject { get; set; }
        public string ObjectId { get; set; }
        public string Owner { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
