using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using PolicyManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PolicyService> _logger;

        public PolicyService(IPolicyRepository policyRepository, IMapper mapper, ILogger<PolicyService> logger)
        {
            _policyRepository = policyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetPolicyViewModel> AddPolicyAsync(PolicyViewModel policyViewModel)
        {
            var policy = _mapper.Map<Policy>(policyViewModel);

            var addedPolicy = await _policyRepository.AddAsync(policy);

            _logger.LogInformation($"Policy has been created - id: {policy.Id}");

            return _mapper.Map<GetPolicyViewModel>(addedPolicy);
        }

        public async Task<bool> DeletePolicyAsync(Guid id)
        {
            var result = await _policyRepository.DeleteByIdAsync(id);

            _logger.LogInformation($"Policy has been deleted - id: {id}");

            return result;
        }

        public async Task<GetPolicyViewModel> FetchPolicyAsync(string policyNumber, ProductType productType)
        {
            var policy = await _policyRepository.FetchByParameterAsync(policyNumber, productType);

            if (policy == null)
            {
                _logger.LogInformation($"No Policy found for the given policy number {policyNumber} and product type {productType.ToString()}");
            }

            var policyViewModel = _mapper.Map<GetPolicyViewModel>(policy);

            return policyViewModel;
        }

        public async Task<IEnumerable<GetPolicyViewModel>> FetchAllPolicyAsync()
        {
            var policyList = await _policyRepository.FetchListAsync();
            var policyViewModelList = new List<GetPolicyViewModel>();

            policyList.ToList().ForEach(policy =>
            {
                var policyViewModel = _mapper.Map<GetPolicyViewModel>(policy);
                policyViewModelList.Add(policyViewModel);
            });

            return policyViewModelList;
        }

        public async Task<GetPolicyViewModel> UpdatePolicyAsync(Guid id, PolicyViewModel policyViewModel)
        {
            var policy = await _policyRepository.FetchByIdAsync(id);

            if(policy != null)
            {
                _logger.LogInformation($"Policy found for an update - id: {id}");
                policy.HasMedicalHistory = policyViewModel.HasMedicalHistory;
                policy.IsSmoker = policyViewModel.IsSmoker;

                var updatedPolicy = await _policyRepository.UpdateByIdAsync(policy.Id, policy);

                _logger.LogInformation($"Policy has been updated - id: {id}");

                return _mapper.Map<GetPolicyViewModel>(updatedPolicy);
            }

            return null;
        }
    }
}
