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
    public class PolicyRepository : CosmosDbRepository<Policy>, IPolicyRepository
    {       
        private readonly ILogger<PolicyRepository> _logger;

        public PolicyRepository(IOptions<AzureCosmosDbOptions> azureCosmosDbOptions, ILogger<PolicyRepository> logger) : base(azureCosmosDbOptions, "Policy")
        {           
            _logger = logger;
        }

        public async Task<Policy> AddAsync(Policy policy)
        {
            //// Need to revisit partitionkey on Milestone2
            return await CreateItemAsync(policy, policy.Id.ToString());
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            return await DeleteItemAsync(id.ToString(), id.ToString());
        }

        public async Task<Policy> FetchByParameterAsync(string policyNumber, ProductType productType)
        {
            var query =
             $"SELECT * FROM c where c.policyNumber = @policyNumber AND c.productType = @productType ";

            var queryDefinition = new QueryDefinition(query).WithParameter("@policyNumber", policyNumber)
                                                            .WithParameter("@productType", productType);

            var response = await FetchItemsAsync(queryDefinition);           

            return response.SingleOrDefault();
        }

        public async Task<IEnumerable<Policy>> FetchListAsync()
        {
            var query =
              $"SELECT * FROM c";

            var queryDefinition = new QueryDefinition(query);

            var response = await FetchItemsAsync(queryDefinition);

            return response;
        }

        public async Task<Policy> UpdateByIdAsync(Guid id, Policy policy)
        {
            if (policy != null)
            {
                //// Need to revisit partitionkey on Milestone2
                return await UpdateItemAsync(policy);
            }

            return null;
        }

        public async Task<Policy> FetchByPolicyNumberAsync(string policyNumber)
        {
            var query =
             $"SELECT * FROM c where c.policyNumber = @policyNumber";

            var queryDefinition = new QueryDefinition(query).WithParameter("@policyNumber", policyNumber);

            var response = await FetchItemsAsync(queryDefinition);

            return response.SingleOrDefault();          
        }

        public async Task<Policy> FetchByIdAsync(Guid id)
        {
            //// Need to revisit partitionkey on Milestone2
            return await FetchItemAsync(id.ToString(), id.ToString());
        }
    }
}
