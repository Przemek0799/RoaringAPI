using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoaringAPI.Model

{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyRating")]
        public int? CompanyRatingId { get; set; }
        public string? RoaringCompanyId { get; set; }
        public string? CompanyName { get; set; } // Nullable
        public DateTime? CompanyRegistrationDate { get; set; } // Nullable
        public int? IndustryCode { get; set; } // Nullable
        public string? IndustryText { get; set; } // Nullable
        public string? LegalGroupCode { get; set; } // Nullable
        public string? LegalGroupText { get; set; } // Nullable
        public bool? EmployerContributionReg { get; set; } // Nullable
        public int? NumberCompanyUnits { get; set; } // Nullable
        public string? NumberEmployeesInterval { get; set; } // Nullable
        public bool? PreliminaryTaxReg { get; set; } // Nullable
        public bool? SeveralCompanyName { get; set; } // Nullable
        public string? Currency { get; set; } // Nullable
        public int? NumberOfEmployees { get; set; } // Nullable
    }
}
