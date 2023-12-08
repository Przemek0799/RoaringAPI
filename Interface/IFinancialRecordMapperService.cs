using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{
    public interface IFinancialRecordMapperService
    {
        Task<FinancialRecord> HandleFinancialRecordAsync(FinancialRecordResponse recordResponse, int companyId);
        Task<Company> HandleCompanyAsync(string roaringCompanyId);
    }
}