using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Lichen.Trace.Abstractions;
using Lichen.Trace.Abstractions.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lichen.Trace.Azure.Table
{
    public class TraceService : ITrace
    {
        private readonly TableServiceClient _tableServiceClient = null;
        private readonly string _tableName;
        private TableClient _tableClient;

        public TraceService(AzureTableOptions options)
        {
            if (string.IsNullOrEmpty(options.TableName))
            {
                return;
            }

            _tableName = options.TableName;

            if (string.IsNullOrEmpty(options.SASUri))
            {
                _tableServiceClient = new TableServiceClient(new Uri(options.SASUri));
            }
            else
            {
                _tableServiceClient = new TableServiceClient(new Uri(options.StorageUri),new TableSharedKeyCredential(options.AccountName, options.AccountKey));
            }
        }

        private async Task<TableClient> GetOrCreateTableClient()
        {
            if (_tableClient != null)
                return _tableClient;

            await _tableServiceClient.CreateTableIfNotExistsAsync(_tableName);
            _tableClient = _tableServiceClient.GetTableClient(_tableName);
            return _tableClient;
        }

        private TableEntity CreateTableEntity(TraceEntity trace)
        {
            if (!trace.Id.HasValue)
            {
                trace.Id = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(trace.ObjectId))
            {
                trace.ObjectId = trace.Id.ToString();
            }

            return new TableEntity(trace.ObjectId, trace.Id.ToString())
            {
                { "Product", "Marker Set" },
                { "Price", 5.00 },
                { "Quantity", 21 }
            };
        }

        public async Task<string> Create(TraceEntity trace)
        {
            var tableClient = await GetOrCreateTableClient();
            var entity = CreateTableEntity(trace);
            await tableClient.AddEntityAsync(entity);
            return trace.Id.ToString();
        }

        public Task Delete(string traceId)
        {
            throw new NotImplementedException();
        }

        public Task<IEquatable<TraceEntity>> List(string objectId)
        {
            throw new NotImplementedException();
        }

        public Task<TraceEntity> Read(string traceId)
        {
            throw new NotImplementedException();
        }

        public Task Update(TraceEntity trace)
        {
            throw new NotImplementedException();
        }
    }
}
