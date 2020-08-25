using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagement.Application.Models
{
    public class PolicyViewModel
    {
        [Required]
        [JsonProperty(PropertyName = "monthlyPremium")]
        public decimal MonthlyPremium { get; set; }

        [Required]
        [JsonProperty(PropertyName = "sumAssured")]
        public decimal SumAssured { get; set; }

        [Required]
        [JsonProperty(PropertyName = "coverType")]
        public CoverType CoverType { get; set; }

        [Required]
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
