using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Interface;

namespace RoaringAPI.Mapping
{
    public class CompanyMapperService : ICompanyMapperService
    {
        private readonly RoaringDbcontext _dbContext;

        public CompanyMapperService(RoaringDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> HandleCompanyAsync(RoaringRecord record)
        {
            var existingCompany = await _dbContext.Companies.FirstOrDefaultAsync(c => c.RoaringCompanyId == record.CompanyId);

            if (existingCompany != null)
            {
                UpdateExistingCompany(existingCompany, record);
            }
            else
            {
                var newCompany = MapRoaringDataToCompany(record);
                _dbContext.Companies.Add(newCompany);
            }

            await _dbContext.SaveChangesAsync();
            return await _dbContext.Companies.FirstOrDefaultAsync(c => c.RoaringCompanyId == record.CompanyId);
        }

        private void UpdateExistingCompany(Company existingCompany, RoaringRecord record)
        {
            if (existingCompany == null || record == null)
                return;

            // Update properties of the company from the RoaringRecord
            existingCompany.CompanyName = record.CompanyName;
            existingCompany.CompanyRegistrationDate = DateTime.ParseExact(record.CompanyRegistrationDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            existingCompany.IndustryCode = int.TryParse(record.IndustryCode, out var industryCode) ? industryCode : 0;
            existingCompany.IndustryText = record.IndustryText;
            existingCompany.LegalGroupCode = record.LegalGroupCode;
            existingCompany.LegalGroupText = record.LegalGroupText;
            existingCompany.EmployerContributionReg = record.EmployerContributionReg;
            existingCompany.NumberCompanyUnits = record.NumberCompanyUnits;
            existingCompany.NumberEmployeesInterval = record.NumberEmployeesInterval;
            existingCompany.PreliminaryTaxReg = record.PreliminaryTaxReg;
            existingCompany.SeveralCompanyName = record.SeveralCompanyName;
            // Update other properties as needed
        }

        private Company MapRoaringDataToCompany(RoaringRecord record)
        {
            if (record == null)
                return null;

            return new Company
            {
                RoaringCompanyId = record.CompanyId,
                CompanyName = record.CompanyName,
                CompanyRegistrationDate = DateTime.ParseExact(record.CompanyRegistrationDate, "yyyyMMdd", CultureInfo.InvariantCulture),
                IndustryCode = int.TryParse(record.IndustryCode, out var industryCode) ? industryCode : 0,
                IndustryText = record.IndustryText,
                LegalGroupCode = record.LegalGroupCode,
                LegalGroupText = record.LegalGroupText,
                EmployerContributionReg = record.EmployerContributionReg,
                NumberCompanyUnits = record.NumberCompanyUnits,
                NumberEmployeesInterval = record.NumberEmployeesInterval,
                PreliminaryTaxReg = record.PreliminaryTaxReg,
                SeveralCompanyName = record.SeveralCompanyName,
                // Map other properties as needed
            };
        }

        // Additional mapping methods for other entities can be added here
    }
}
