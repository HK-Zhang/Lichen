using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Lichen.Trace.Abstractions;
using Lichen.Trace.Abstractions.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;

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

            if (!trace.Timestamp.HasValue)
            {
                trace.Timestamp = DateTime.UtcNow;
            }

            return new TableEntity(trace.ObjectId, trace.Id.ToString())
            {
                { "Id", trace.Id.Value },
                { "ObjectId", trace.ObjectId },
                { "Log", trace.Log },
                { "Timestamp", trace.Timestamp.Value },
                { "Owner", trace.Owner }
            };
        }

        public async Task<string> Create(TraceEntity trace)
        {
            if (string.IsNullOrEmpty(trace.Log))
            {
                if (string.IsNullOrEmpty(trace.Log)) return null;
                trace.Log = JsonSerializer.Serialize(trace.LogInObject);
            }
            var tableClient = await GetOrCreateTableClient();
            var entity = CreateTableEntity(trace);
            await tableClient.AddEntityAsync(entity);
            return trace.Id.ToString();
        }

        public async Task Delete(string traceId)
        {
            var tableClient = await GetOrCreateTableClient();

            await tableClient.DeleteEntityAsync(traceId, traceId);
        }

        public async Task Delete(string objectId, string traceId)
        {
            var tableClient = await GetOrCreateTableClient();

            await tableClient.DeleteEntityAsync(objectId, traceId);
        }

        public async Task<IEnumerable<TraceEntity>> List(string objectId)
        {
            var tableClient = await GetOrCreateTableClient();
            AsyncPageable<TableEntity> queryResults = tableClient.QueryAsync<TableEntity>(filter: $"PartitionKey eq '{objectId}'");

            var result = new List<TraceEntity>();

            await foreach (TableEntity qEntity in queryResults)
            {
                result.Add(new TraceEntity
                {
                    Log = qEntity.GetString("Log"),
                    ObjectId = objectId,
                    Timestamp = qEntity.GetDateTime("Timestamp"),
                    Owner = qEntity.GetString("Owner"),
                    Id = qEntity.GetGuid("Id")
                });
            }

            return result;
        }

        public async Task<TraceEntity> Read(string objectId,string traceId)
        {
            var tableClient = await GetOrCreateTableClient();
            var response = await tableClient.GetEntityAsync<TableEntity>(objectId, traceId);
            if(response.Value == null) return null;

            return new TraceEntity
            {
                Log = response.Value.GetString("Log"),
                ObjectId = objectId,
                Timestamp = response.Value.GetDateTime("Timestamp"),
                Owner = response.Value.GetString("Owner"),
                Id = response.Value.GetGuid("Id")
            };
        }

        public async Task<TraceEntity> Read(string traceId)
        {
            return await Read(traceId, traceId);
        }

        public Task Update(TraceEntity trace)
        {
            throw new NotImplementedException();
        }
    }
}
