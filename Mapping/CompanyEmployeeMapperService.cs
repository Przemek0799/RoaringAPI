using Microsoft.EntityFrameworkCore;
using RoaringAPI.Interface;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;
using System.Threading.Tasks;

namespace RoaringAPI.Mapping
{
    public class CompanyEmployeeMapperService : ICompanyEmployeeMapperService
    {
        private readonly RoaringDbcontext _dbContext;

        public CompanyEmployeeMapperService(RoaringDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CompanyEmployee> HandleCompanyEmployeeAsync(RoaringRecord record, int companyId)
        {
            var existingEmployee = await _dbContext.CompanyEmployees.FirstOrDefaultAsync(e => e.CompanyId == companyId);

            if (existingEmployee != null)
            {
                UpdateExistingEmployee(existingEmployee, record);
            }
            else
            {
                var companyemployee = MapRoaringDataToCompanyEmployee(record, companyId);
                _dbContext.CompanyEmployees.Add(companyemployee);
            }

            await _dbContext.SaveChangesAsync();
            return await _dbContext.CompanyEmployees.FirstOrDefaultAsync(e => e.CompanyId == companyId);
        }

        private void UpdateExistingEmployee(CompanyEmployee existingEmployee, RoaringRecord record)
        {
            if (existingEmployee == null || record == null)
                return;

            existingEmployee.TopDirectorFunction = record.TopDirectorFunction;
            existingEmployee.TopDirectorName = record.TopDirectorName;
            // Update other properties as needed
        }

        private CompanyEmployee MapRoaringDataToCompanyEmployee(RoaringRecord record, int companyId)
        {
            if (record == null)
                return null;

            return new CompanyEmployee
            {
                CompanyId = companyId,
                TopDirectorFunction = record.TopDirectorFunction,
                TopDirectorName = record.TopDirectorName,
                // Map other properties as needed
            };
        }

        // Additional mapping methods for other entities can be added here
    }
}
