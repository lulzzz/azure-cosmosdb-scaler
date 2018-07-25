﻿using Microsoft.Azure.Documents.Client;

namespace NoOpsJp.CosmosDbScaler.Clients
{
    public class StreamlinedDocumentClientOptions
    {
        public string AccountEndpoint { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseId { get; set; }
        public ConnectionPolicy ConnectionPolicy { get; set; }

    }
}
