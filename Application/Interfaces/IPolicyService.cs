using Application.Models;
using Domain.Entities;
using PolicyManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPolicyService
    {
        Task<GetPolicyViewModel> AddPolicyAsync(
           PolicyViewModel policyViewModel);

        Task<PolicyViewModel> DeletePolicyAsync(
            int id);

        Task<GetPolicyViewModel> FetchPolicyAsync(
            string policyNumber, ProductType productType);

        Task<IEnumerable<GetPolicyViewModel>> FetchAllPolicyAsync();

        Task<GetPolicyViewModel> UpdatePolicyAsync(
            Guid id, PolicyViewModel policyViewModel);
    }
}
