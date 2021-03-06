﻿using NoOpsJp.CosmosDbScaler.Clients;

namespace todo
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class DocumentDBRepository<T> : IDocumentDBRepository<T> where T : class
    {
        private StreamlinedDocumentClient client;

        public DocumentDBRepository(StreamlinedDocumentClient streamlinedDocumentClient)
        {
            client = streamlinedDocumentClient;
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                return await client.ReadDocumentAsync<T>("Items", id);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = client.CreateDocumentQuery<T>("Items", new FeedOptions { MaxItemCount = -1 })
                              .Where(predicate)
                              .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await client.ExecuteQueryAsync(query));
            }

            return results;
        }

        public async Task CreateItemAsync(T item)
        {
            await client.CreateDocumentAsync("Items", item);
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            await client.ReplaceDocumentAsync("Items", id, item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync("Items", id);
        }
    }

    public class DocumentDBOptions
    {
        public string AccountEndpoint { get; set; }
        public string AccountKeys { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}