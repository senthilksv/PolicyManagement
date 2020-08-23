using Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Policy : AuditableBaseEntity
    {
        public decimal MonthlyPremium { get; set; }
        public decimal SumAssured { get; set; }

        public string CoverType { get; set; }
    }
}
