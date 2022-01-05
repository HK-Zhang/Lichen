using System;
using System.Collections.Generic;
using System.Text;

namespace Lichen.Trace.Azure.Table
{
    public class AzureTableOptions
    {
        public string StorageUri { get; set; }
        public string SASUri { get; set; }
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string TableName { get; set; }
    }
}
