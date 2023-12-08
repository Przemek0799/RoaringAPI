using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{
   
        public interface IGroupStructureMapperService
        {
            Task<Company> HandleCompanyAsync(string roaringCompanyId, string companyName);
            Task<CompanyStructure> HandleCompanyStructureAsync(int companyId, string motherCompanyId, GroupCompanyResponse groupCompany);
        }
  

}

