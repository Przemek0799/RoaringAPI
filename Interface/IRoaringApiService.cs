using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{
    public interface IRoaringApiService
    {
        Task<FinancialRecordApiResponse> FetchCompanyFinancialRecordAsync(string companyId);
        Task<RoaringApiResponse> FetchDataAsync(string companyId);
        Task<RoaringApiResponse> FetchCompanyGroupStructureAsync(string companyId);
        Task<CompanyRatingApiRespons> FetchCompanyRatingAsync(string companyId);
        Task<RoaringSearchResult> FetchCompanyByFreeTextAsync(string freeText);

    }
}
