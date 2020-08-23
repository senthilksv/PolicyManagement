using Application.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public abstract class CosmosDbRepository : IDbRepository
    {
        private readonly IOptions<AzureCosmosDbOptions> _azureCosmosDbOptions;
        private readonly CosmosClient _cosmosClient;
        protected readonly Database _cosmosDatabase;

        public CosmosDbRepository(IOptions<AzureCosmosDbOptions> azureCosmosDbOptions)
        {
            _azureCosmosDbOptions = azureCosmosDbOptions;

            _cosmosClient = new CosmosClient(_azureCosmosDbOptions.Value.Endpoint, _azureCosmosDbOptions.Value.Key);

            _cosmosDatabase = _cosmosClient.GetDatabase(_azureCosmosDbOptions.Value.DatabaseId);
        }
    }
}
