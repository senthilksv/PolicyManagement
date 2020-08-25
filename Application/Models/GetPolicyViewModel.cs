using Domain.Entities;
using Newtonsoft.Json;
using PolicyManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class GetPolicyViewModel
    {        
        [JsonProperty(PropertyName = "monthlyPremium")]
        public decimal MonthlyPremium { get; set; }

        [JsonProperty(PropertyName = "policyNumber")]
        public string PolicyNumber { get; set; }
               
        [JsonProperty(PropertyName = "sumAssured")]
        public decimal SumAssured { get; set; }
               
        [JsonProperty(PropertyName = "coverType")]
        public CoverType CoverType { get; set; }
               
        [JsonProperty(PropertyName = "productType")]
        public ProductType ProductType { get; set; }

        [JsonProperty(PropertyName = "customerDetails")]
        public CustomerViewModel CustomerDetails { get; set; }

        [JsonProperty(PropertyName = "isSmoker")]
        public bool IsSmoker { get; set; }

        [JsonProperty(PropertyName = "hasMedicalHistory")]
        public bool HasMedicalHistory { get; set; }
    }
}
