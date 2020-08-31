using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPolicyRepository : IDbRepository
    {
        Task<Policy> AddAsync(
            Policy policy);

        Task<bool> DeleteByIdAsync(
            Guid id);

        Task<Policy> FetchByParameterAsync(
            string policyNumber, ProductType productType);

        Task<Policy> FetchByPolicyNumberAsync(
          string policyNumber);

        Task<Policy> FetchByIdAsync(
         Guid id);

        Task<IEnumerable<Policy>> FetchListAsync();

        Task<Policy> UpdateByIdAsync(
            Guid id, Policy policy);
    }
}
