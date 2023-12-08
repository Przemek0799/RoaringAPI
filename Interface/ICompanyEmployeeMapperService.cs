using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{

    public interface ICompanyEmployeeMapperService
    {
        Task<CompanyEmployee> HandleCompanyEmployeeAsync(RoaringRecord record, int companyId);
    }

}

