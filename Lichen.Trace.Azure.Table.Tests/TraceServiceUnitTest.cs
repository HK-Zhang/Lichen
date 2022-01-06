using Lichen.Trace.Abstractions.Entity;
using System.Threading.Tasks;
using Xunit;

namespace Lichen.Trace.Azure.Table.Tests
{
    public class TraceServiceUnitTest
    {
        private AzureTableOptions option1 = new AzureTableOptions()
        {
            TableName = "trace",
            SASUri = "*",
        };

        private AzureTableOptions option2 = new AzureTableOptions()
        {
            TableName = "trace",
            AccountKey = "",
            AccountName = "",
            StorageUri = ""
        };

        private TraceEntity ExpectedTraceEntity = new TraceEntity()
        {
            Log = "Where am I?",
            ObjectId = "lichen",
            Owner = "zhk"
        };

        [Fact]
        public async Task CreateTraceAsync()
        {
            var ts = new TraceService(option2);
            var id = await ts.Create(ExpectedTraceEntity);
            Assert.NotNull(id);
        }

        [Fact]
        public async Task CreateTraceAsync2()
        {
            var ts = new TraceService(option1);
            var id = await ts.Create(ExpectedTraceEntity);
            Assert.NotNull(id);
        }

        [Fact]
        public async Task CreateReadDeleteTraceAsync()
        {
            var ts = new TraceService(option1);
            var id = await ts.Create(ExpectedTraceEntity);
            Assert.NotNull(id);

            var entity = await ts.Read(ExpectedTraceEntity.ObjectId, id);
            Assert.Equal(ExpectedTraceEntity.Log,entity.Log);

            await ts.Delete(ExpectedTraceEntity.ObjectId, id);

            var entityNew = await ts.Read(ExpectedTraceEntity.ObjectId, id);
            Assert.Null(entityNew);
        }

        [Fact]
        public async Task ListTraceAsync()
        {
            var ts = new TraceService(option1);
            await ts.Create(ExpectedTraceEntity);

            var entityList = await ts.List(ExpectedTraceEntity.ObjectId);
            Assert.NotEmpty(entityList);
        }
    }
}