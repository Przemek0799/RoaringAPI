using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Interface
{
   
        public interface IAddressMapperService
        {
            Task<Address> HandleAddressAsync(RoaringRecord record, int companyId);
        }

    }

