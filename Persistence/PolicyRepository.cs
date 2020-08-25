using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class PolicyRepository : CosmosDbRepository, IPolicyRepository
    {
        private readonly Container _policyContainer;
        private readonly ILogger<PolicyRepository> _logger;

        public PolicyRepository(IOptions<AzureCosmosDbOptions> azureCosmosDbOptions, ILogger<PolicyRepository> logger) : base(azureCosmosDbOptions)
        {
            _policyContainer = _cosmosDatabase.GetContainer("Policy");
            _logger = logger;
        }

        public async Task<Policy> AddAsync(Policy policy)
        {
            //// Need to revisit partitionkey on Milestone2
            return await _policyContainer.CreateItemAsync<Policy>(policy, new PartitionKey(policy.Id.ToString()));
        }

        public async Task<Policy> DeleteByIdAsync(Guid id)
        {
            return await this._policyContainer.DeleteItemAsync<Policy>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<Policy> FetchByParameterAsync(string policyNumber, ProductType productType)
        {
            var query =
             $"SELECT * FROM c where c.policyNumber = @policyNumber AND c.productType = @productType ";

            var queryDefinition = new QueryDefinition(query).WithParameter("@policyNumber", policyNumber)
                                                            .WithParameter("@productType", productType);

            var queryIterator = _policyContainer.GetItemQueryIterator<Policy>(queryDefinition);

            var response = await queryIterator.ReadNextAsync();

            return response.Resource.SingleOrDefault();
        }

        public async Task<IEnumerable<Policy>> FetchListAsync()
        {
            var query =
              $"SELECT * FROM c";

            var queryDefinition = new QueryDefinition(query);

            var queryIterator = _policyContainer.GetItemQueryIterator<Policy>(queryDefinition);

            var policyList = new List<Policy>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                policyList.AddRange(response);
            }

            return policyList;
        }

        public async Task<Policy> UpdateByIdAsync(Guid id, Policy policy)
        {
            if (policy != null)
            {
                //// Need to revisit partitionkey on Milestone2
                return await this._policyContainer.ReplaceItemAsync<Policy>(policy, id.ToString());
            }

            return null;
        }

        public async Task<Policy> FetchByPolicyNumberAsync(string policyNumber)
        {
            var query =
             $"SELECT * FROM c where c.policyNumber = @policyNumber";

            var queryDefinition = new QueryDefinition(query).WithParameter("@policyNumber", policyNumber);

            var queryIterator = _policyContainer.GetItemQueryIterator<Policy>(queryDefinition);

            var response = await queryIterator.ReadNextAsync();

            return response.Resource.SingleOrDefault();
        }

        public async Task<Policy> FetchByIdAsync(Guid id)
        {
            //// Need to revisit partitionkey on Milestone2
            try
            {
                return await _policyContainer.ReadItemAsync<Policy>(id.ToString(), new PartitionKey(id.ToString()));
            }
            catch(CosmosException exception)
            {
                _logger.LogDebug("No Item found", exception.Message);
                _logger.LogInformation($"No item found for the given id: {id}");
                return null;
            }
        }
    }
}
