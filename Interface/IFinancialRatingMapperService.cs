using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{
    public interface IFinancialRatingMapperService
    {
        Task<CompanyRating> CreateOrUpdateCompanyRatingAsync(string roaringCompanyId, CompanyRatingApiRespons companyRatingData);
        Task<Company> HandleCompanyAsync(string roaringCompanyId, int companyRatingId);
    }
}
