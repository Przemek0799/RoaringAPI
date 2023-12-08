// File: Mapping/GroupStructureMapperService.cs
using RoaringAPI.Model;
using System.Linq;
using System.Threading.Tasks;
using RoaringAPI.ModelRoaring;
using RoaringAPI.Interface;
using Microsoft.Extensions.Logging;

namespace RoaringAPI.Mapping
{
    public class GroupStructureMapperService : IGroupStructureMapperService
    {
        private readonly RoaringDbcontext _dbContext;
        private readonly ILogger<GroupStructureMapperService> _logger;

        public GroupStructureMapperService(RoaringDbcontext dbContext, ILogger<GroupStructureMapperService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Company> HandleCompanyAsync(string roaringCompanyId, string companyName)
        {
            var company = _dbContext.Companies.FirstOrDefault(c => c.RoaringCompanyId == roaringCompanyId);

            if (company == null)
            {
                company = new Company
                {
                    RoaringCompanyId = roaringCompanyId,
                    CompanyName = companyName,
                    // Other properties
                };
                _dbContext.Companies.Add(company);
                _logger.LogInformation($"Added new company with RoaringCompanyId: {roaringCompanyId}");
            }
            else if (!string.IsNullOrWhiteSpace(companyName) && company.CompanyName != companyName)
            {
                company.CompanyName = companyName; // Update existing company details
                _logger.LogInformation($"Updated company with RoaringCompanyId: {roaringCompanyId}");
            }

            await _dbContext.SaveChangesAsync();
            return company;
        }

        public async Task<CompanyStructure> HandleCompanyStructureAsync(int companyId, string motherCompanyId, GroupCompanyResponse groupCompanyResponse)
        {
            _logger.LogInformation($"Handling Company Structure for companyId: {companyId}, rawMotherCompanyId: '{motherCompanyId}'");

            int? parsedMotherCompanyId = null;
            if (!string.IsNullOrEmpty(motherCompanyId))
            {
                // First, ensure the mother company exists in the database.
                var motherCompany = await EnsureCompanyExists(motherCompanyId);
                parsedMotherCompanyId = motherCompany?.CompanyId;
            }

            if (parsedMotherCompanyId.HasValue)
            {
                var companyStructure = _dbContext.CompanyStructures
                    .FirstOrDefault(cs => cs.CompanyId == companyId && cs.MotherCompanyId == parsedMotherCompanyId);

                if (companyStructure == null)
                {
                    companyStructure = new CompanyStructure
                    {
                        CompanyId = companyId,
                        MotherCompanyId = parsedMotherCompanyId.Value,
                        CompanyLevel = groupCompanyResponse.CompanyLevel,
                        OwnedPercentage = groupCompanyResponse.OwnedPercentage,
                    };
                    _dbContext.CompanyStructures.Add(companyStructure);
                    _logger.LogInformation($"Adding new CompanyStructure: CompanyId={companyId}, MotherCompanyId={parsedMotherCompanyId.Value}");
                }
                else
                {
                    companyStructure.CompanyLevel = groupCompanyResponse.CompanyLevel;
                    companyStructure.OwnedPercentage = groupCompanyResponse.OwnedPercentage;
                    _logger.LogInformation($"Updating CompanyStructure: CompanyId={companyId}, MotherCompanyId={parsedMotherCompanyId.Value}");
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"No valid motherCompanyId found for rawMotherCompanyId: '{motherCompanyId}', skipping CompanyStructure creation.");
            }

            return null; // Return null if no valid motherCompanyId is found or CompanyStructure is created/updated
        }

        private async Task<Company> EnsureCompanyExists(string roaringCompanyId)
        {
            var company = _dbContext.Companies.FirstOrDefault(c => c.RoaringCompanyId == roaringCompanyId);
            if (company == null)
            {
                // Create the company if it doesn't exist
                company = new Company { RoaringCompanyId = roaringCompanyId };
                _dbContext.Companies.Add(company);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Created new company with RoaringCompanyId: {roaringCompanyId}");
            }

            return company;
        }




    }
}

