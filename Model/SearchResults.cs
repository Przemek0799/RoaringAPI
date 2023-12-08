using RoaringAPI.Model;
using System.Collections.Generic;

namespace RoaringAPI.Models
{
    public class SearchResults
    {
        public List<Address> Addresses { get; set; }
        public List<Company> Companies { get; set; }
        public List<CompanyEmployee> CompanyEmployees { get; set; }
        public List<CompanyRating> CompanyRatings { get; set; }
        public List<CompanyStructure> CompanyStructures { get; set; }
        public List<FinancialRecord> FinancialRecords { get; set; }
    }
}
