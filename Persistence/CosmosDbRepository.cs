using Application.Common;
using Application.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public abstract class CosmosDbRepository<T> where T : AuditableBaseEntity
    {
        private readonly IOptions<AzureCosmosDbOptions> _azureCosmosDbOptions;
        private readonly CosmosClient _cosmosClient;
        private readonly Database _cosmosDatabase;
        protected readonly Container _cosmosContainer;

        protected CosmosDbRepository(IOptions<AzureCosmosDbOptions> azureCosmosDbOptions, string container)
        {
            _azureCosmosDbOptions = azureCosmosDbOptions;

            _cosmosClient = new CosmosClient(_azureCosmosDbOptions.Value.Endpoint, _azureCosmosDbOptions.Value.Key);

            _cosmosDatabase = _cosmosClient.GetDatabase(_azureCosmosDbOptions.Value.DatabaseId);

            _cosmosContainer = _cosmosDatabase.GetContainer(container);
        }        

        protected virtual async Task<T> CreateItemAsync(T item, string partitionKeyValue)
        {
            return await _cosmosContainer.CreateItemAsync<T>(item, new PartitionKey(partitionKeyValue));
        }

        protected virtual async Task<bool> DeleteItemAsync(string id, string partitionKeyValue)
        {
            var response = await this._cosmosContainer.DeleteItemAsync<T>(id, new PartitionKey(partitionKeyValue));

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;                   
        }

        protected virtual async Task<IEnumerable<T>> FetchItemsAsync(QueryDefinition queryDefinition)
        {
            var queryIterator = _cosmosContainer.GetItemQueryIterator<T>(queryDefinition);

            var itemList = new List<T>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                itemList.AddRange(response);
            }

            return itemList;
        }

        protected virtual async Task<T> UpdateItemAsync(T item)
        {
            return await this._cosmosContainer.ReplaceItemAsync<T>(item, item.Id.ToString());
        }

        protected virtual async Task<T> FetchItemAsync(string id, string partitionKeyValue = "")
        {
            if (!string.IsNullOrEmpty(partitionKeyValue))
            {
              return   await _cosmosContainer.ReadItemAsync<T>(id, new PartitionKey(partitionKeyValue));
            }

            return await _cosmosContainer.ReadItemAsync<T>(id, new PartitionKey());
        }
    }
}
