using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lichen.Trace.Abstractions.Entity;

namespace Lichen.Trace.Abstractions
{
    public interface ITrace
    {
        Task<string> Create(TraceEntity trace);
        Task<TraceEntity> Read(string traceId);
        Task<TraceEntity> Read(string objectId,string traceId);
        Task Update(TraceEntity trace);
        Task Delete(string traceId);
        Task Delete(string objectId, string traceId);
        Task<IEnumerable<TraceEntity>> List(string objectId);
    }
}
