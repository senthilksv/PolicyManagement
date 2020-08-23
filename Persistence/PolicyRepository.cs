using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Persistence
{
    public class PolicyRepository : CosmosDbRepository, IPolicyRepository
    {
        private readonly Container _policyContainer;
        public PolicyRepository(IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
            _policyContainer = _cosmosDatabase.GetContainer("Policy");
        }

        public async Task<Policy> AddAsync(Policy policy)
        {
            return await _policyContainer.CreateItemAsync<Policy>(policy, new PartitionKey());
        }

        public async Task<Policy> DeleteByIdAsync(int id)
        {
            return await this._policyContainer.DeleteItemAsync<Policy>(id.ToString(), new PartitionKey(id));
        }

        public async Task<Policy> FetchByIdAsync(int id)
        {
            try
            {
                ItemResponse<Policy> response = await this._policyContainer.ReadItemAsync<Policy>(id.ToString(), new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Policy>> FetchListAsync()
        {
            var query =
              $"SELECT * FROM p";

            var queryDefinition = new QueryDefinition(query);

            var queryIterator = _policyContainer.GetItemQueryIterator<Policy>(queryDefinition);

            var policyList = new List<Policy>();

            while(queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                policyList.AddRange(response);
            }

            return policyList;
        }

        public async Task<Policy> UpdateByIdAsync(int id)
        {
            var policy = await FetchByIdAsync(id);
            if (policy != null)
            {
               return await this._policyContainer.UpsertItemAsync<Policy>(policy, new PartitionKey(id));
            }

            return null;
        }
    }
}
