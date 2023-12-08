using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoaringAPI.Model
{
    public class CompanyStructure
    {
        [Key]
        public int CompanyStructureId { get; set; }

        // Foreign key for Company (assuming this is the subsidiary or the actual company)
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey("MotherCompany")]
        public int? MotherCompanyId { get; set; }

        // Navigation property for MotherCompany (also a Company type)
        public Company MotherCompany { get; set; }

        public int CompanyLevel { get; set; }
        public double OwnedPercentage { get; set; }

    }
}
