using Domain.Entities;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Policy.Queries
{
    public class GetAllPolicyQuery : IRequest<IEnumerable<Domain.Entities.Policy>>
    {
        public class GetAllProductsQueryHandler : IRequestHandler<GetAllPolicyQuery, IEnumerable<Domain.Entities.Policy>>
        {
            private readonly IPolicyRepositoryAsync _repository;
            public GetAllProductsQueryHandler(IPolicyRepositoryAsync repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Domain.Entities.Policy>> Handle(GetAllPolicyQuery query, CancellationToken cancellationToken)
            {
                var policies = await _repository.GetAllAsync();
                if (policies == null)
                {
                    return null;
                }

                return policies;
            }
        }
    }
}
