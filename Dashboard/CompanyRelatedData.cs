using RoaringAPI.Model;

namespace RoaringAPI.Dashboard
{
    public class CompanyRelatedData
    {
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<CompanyEmployee> CompanyEmployees { get; set; }
        public IEnumerable<FinancialRecord> FinancialRecords { get; set; }
        public IEnumerable<CompanyRating> CompanyRatings { get; set; }
        public IEnumerable<CompanyStructure> CompanyStructures { get; set; }


    }
}
