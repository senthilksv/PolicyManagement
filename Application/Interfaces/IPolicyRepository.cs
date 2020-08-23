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

        Task<Policy> DeleteByIdAsync(
            int id);

        Task<Policy> FetchByIdAsync(
            int id);

        Task<IEnumerable<Policy>> FetchListAsync();

        Task<Policy> UpdateByIdAsync(
            int id);
    }
}
