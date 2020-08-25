using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
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

        public PolicyService(IPolicyRepository policyRepository, IMapper mapper)
        {
            _policyRepository = policyRepository;
            _mapper = mapper;
        }

        public async Task<GetPolicyViewModel> AddPolicyAsync(PolicyViewModel policyViewModel)
        {
            var policy = _mapper.Map<Policy>(policyViewModel);

            var addedPolicy = await _policyRepository.AddAsync(policy);

            return _mapper.Map<GetPolicyViewModel>(addedPolicy);
        }

        public Task<PolicyViewModel> DeletePolicyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPolicyViewModel> FetchPolicyAsync(string policyNumber, ProductType productType)
        {
            var policy = await _policyRepository.FetchByParameterAsync(policyNumber, productType);

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
                policy.HasMedicalHistory = policyViewModel.HasMedicalHistory;
                policy.IsSmoker = policyViewModel.IsSmoker;

                var updatedPolicy = await _policyRepository.UpdateByIdAsync(policy.Id, policy);

                return _mapper.Map<GetPolicyViewModel>(updatedPolicy);
            }

            return null;
        }
    }
}
