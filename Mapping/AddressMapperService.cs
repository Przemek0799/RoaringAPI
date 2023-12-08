using Microsoft.EntityFrameworkCore;
using RoaringAPI.Interface;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;
using System.Threading.Tasks;

namespace RoaringAPI.Mapping
{
    public class AddressMapperService : IAddressMapperService
    {
        private readonly RoaringDbcontext _dbContext;

        public AddressMapperService(RoaringDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Address> HandleAddressAsync(RoaringRecord record, int companyId)
        {
            var existingAddress = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.CompanyId == companyId);

            if (existingAddress != null)
            {
                UpdateExistingAddress(existingAddress, record);
            }
            else
            {
                var address = MapRoaringDataToAddress(record, companyId);
                _dbContext.Addresses.Add(address);
            }

            await _dbContext.SaveChangesAsync();
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.CompanyId == companyId);
        }

        private void UpdateExistingAddress(Address existingAddress, RoaringRecord record)
        {
            if (existingAddress == null || record == null)
                return;

            // Update properties of the address from the RoaringRecord
            existingAddress.AddressLine = record.Address;
            existingAddress.CoAddress = record.CoAddress;
            existingAddress.Commune = record.Commune;
            existingAddress.CommuneCode = int.TryParse(record.CommuneCode, out int communeCode) ? communeCode : 0;
            existingAddress.County = record.County;
            existingAddress.ZipCode = int.TryParse(record.ZipCode, out int zipCode) ? zipCode : 0;
            existingAddress.Town = record.Town;
            // Update other properties as needed
        }

        private Address MapRoaringDataToAddress(RoaringRecord record, int companyId)
        {
            if (record == null)
                return null;

            return new Address
            {
                CompanyId = companyId,
                AddressLine = record.Address,
                CoAddress = record.CoAddress,
                Commune = record.Commune,
                CommuneCode = int.TryParse(record.CommuneCode, out int communeCode) ? communeCode : 0,
                County = record.County,
                ZipCode = int.TryParse(record.ZipCode, out int zipCode) ? zipCode : 0,
                Town = record.Town,
                // Map other properties as needed
            };
        }

        // Additional mapping methods for other entities can be added here
    }
}
