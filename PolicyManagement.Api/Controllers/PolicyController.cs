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

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPolicyViewModel>))]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _policyService.FetchAllPolicyAsync();

            return Ok(result);
        }

        [HttpGet("{policyNumber}/{productType}", Name = "Get")]
        [ProducesResponseType(200, Type = typeof(GetPolicyViewModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Get([FromRoute] string policyNumber, [FromRoute] ProductType productType)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                _logger.LogInformation("Policy Number or Product type can not be blank");
                return BadRequest("Policy Number or Product type can not be blank");
            }

            var result = await _policyService.FetchPolicyAsync(policyNumber, productType);

            return Ok(result);
        }

        [HttpPost(Name = "Add")]
        [ProducesResponseType(201, Type = typeof(GetPolicyViewModel))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        public async Task<IActionResult> AddAsync(
            [FromBody] PolicyViewModel policyViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Add action Model state validaiton failed", ModelState);
                return BadRequest(ModelState);
            }

            var result = await _policyService.AddPolicyAsync(policyViewModel);

            return Ok(result);
        }

        [HttpPut("{id}", Name = "Update")]
        [ProducesResponseType(201, Type = typeof(GetPolicyViewModel))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,
           [FromBody] PolicyViewModel policyViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Update action Model state validaiton failed", ModelState);
                return BadRequest(ModelState);
            }

            if (id == null
                || id == Guid.Empty)
            {
                _logger.LogInformation("Id required");
                return BadRequest("Id required.");
            }

            var result = await _policyService.UpdatePolicyAsync(id, policyViewModel);

            if (result == null)
            {
                _logger.LogInformation($"No Policy Found for the given Id :{id}");
                return BadRequest("Policy not found for an update");
            }

            return Ok(result);
        }

        [HttpDelete("{id}", Name = "Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteByIdAsync(
           [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                _logger.LogInformation("Id required");
                return BadRequest("Id required.");
            }

            var result = await _policyService.DeletePolicyAsync(
                 id);

            if (result)
            {
                return Ok("Item has been deleted");
            }

            return BadRequest("Unable to delete.");
        }
    }
}
