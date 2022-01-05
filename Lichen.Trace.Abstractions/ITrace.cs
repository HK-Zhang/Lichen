using System;
using System.Threading.Tasks;
using Lichen.Trace.Abstractions.Entity;

namespace Lichen.Trace.Abstractions
{
    public interface ITrace
    {
        Task<string> Create(TraceEntity trace);
        Task<TraceEntity> Read(string traceId);
        Task Update(TraceEntity trace);
        Task Delete(string traceId);
        Task<IEquatable<TraceEntity>> List(string objectId);
    }
}
