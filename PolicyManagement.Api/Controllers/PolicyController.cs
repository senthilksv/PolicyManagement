using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using PolicyManagement.Application.Models;

namespace PolicyManagement.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly ILogger<PolicyController> _logger;
        private readonly IPolicyService _policyService;

        public PolicyController(ILogger<PolicyController> logger, IPolicyService policyService)
        {
            _logger = logger;
            _policyService = policyService;
        }

        [HttpGet(Name = "FetchPolicies")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPolicyViewModel>))]
        public async Task<IActionResult> FetchListAsync()
        {
            var result = await _policyService.FetchAllPolicyAsync();

            return Ok(result);
        }

        [HttpGet("{policyNumber}/{productType}", Name = "FetchPolicy")]
        [ProducesResponseType(200, Type = typeof(GetPolicyViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Get([FromRoute] string policyNumber, [FromRoute] ProductType productType)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return BadRequest("Policy Number or Product type can not be blank");
            }

            var result = await _policyService.FetchPolicyAsync(policyNumber, productType);

            return Ok(result);
        }

        [HttpPost(Name = "AddPolicy")]
        [ProducesResponseType(201, Type = typeof(GetPolicyViewModel))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        public async Task<IActionResult> AddAsync(
            [FromBody] PolicyViewModel policyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _policyService.AddPolicyAsync(policyViewModel);

            return Ok(result);
        }

        [HttpPut("", Name = "UpdatePolicy")]
        [ProducesResponseType(201, Type = typeof(GetPolicyViewModel))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync([FromQuery] Guid id,
           [FromBody] PolicyViewModel policyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _policyService.UpdatePolicyAsync(id, policyViewModel);

            if(result == null)
            {
                return BadRequest("Policy not found for an update");
            }

            return Ok(result);
        }
    }
}
