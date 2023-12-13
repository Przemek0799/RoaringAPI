using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{

    public interface ICompanyMapperService
    {
        Task<Company> HandleCompanyAsync(RoaringRecord record);

    }

}

