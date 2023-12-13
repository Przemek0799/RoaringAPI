using Microsoft.EntityFrameworkCore;
using RoaringAPI.Interface;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Mapping
{
    public class FinancialRatingMapperService : IFinancialRatingMapperService
    {
        private readonly RoaringDbcontext _dbContext;

        public FinancialRatingMapperService(RoaringDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public CompanyRating MapCompanyRating(CompanyRatingApiRespons companyRatingData)
        {
            if (companyRatingData == null)
                return null;

            var companyRating = new CompanyRating
            {
                Commentary = companyRatingData.Commentary,
                CreditLimit = companyRatingData.CreditLimit,
                Currency = companyRatingData.Currency,
                Rating = companyRatingData.Rating,
                RatingText = companyRatingData.RatingText,
                RiskPrognosis = companyRatingData.RiskPrognosis,
                // Fields from the Rejection object
                CauseOfReject = companyRatingData.Rejection?.CauseOfReject,
                RejectComment = companyRatingData.Rejection?.RejectComment,
                RejectText = companyRatingData.Rejection?.RejectText
            };

            return companyRating;
        }

        public async Task<CompanyRating> CreateOrUpdateCompanyRatingAsync(string roaringCompanyId, CompanyRatingApiRespons companyRatingData)
        {
            if (companyRatingData == null)
                return null;

            var existingCompany = await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.RoaringCompanyId == roaringCompanyId);

            CompanyRating existingCompanyRating = null;
            if (existingCompany != null && existingCompany.CompanyRatingId.HasValue)
            {
                existingCompanyRating = await _dbContext.CompanyRatings
                    .FindAsync(existingCompany.CompanyRatingId.Value);
            }

            if (existingCompanyRating != null)
            {
                // Update existing rating
                UpdateCompanyRating(existingCompanyRating, companyRatingData);
                await _dbContext.SaveChangesAsync();
                return existingCompanyRating;
            }
            else
            {
                // Create new rating
                var newRating = MapCompanyRating(companyRatingData);
                _dbContext.CompanyRatings.Add(newRating);
                await _dbContext.SaveChangesAsync();

                if (existingCompany != null)
                {
                    existingCompany.CompanyRatingId = newRating.CompanyRatingId;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    // Create new company if it doesn't exist
                    var newCompany = new Company
                    {
                        RoaringCompanyId = roaringCompanyId,
                        CompanyRatingId = newRating.CompanyRatingId,
                        // Populate other properties if necessary
                    };

                    _dbContext.Companies.Add(newCompany);
                    await _dbContext.SaveChangesAsync();
                }
                return newRating;
            }
        }



        public void UpdateCompanyRating(CompanyRating companyRating, CompanyRatingApiRespons companyRatingData)
        {
            if (companyRating == null || companyRatingData == null)
                return;

            // Update properties of companyRating from companyRatingData
            companyRating.CauseOfReject = companyRatingData.Rejection?.CauseOfReject;
            companyRating.RejectComment = companyRatingData.Rejection?.RejectComment;
            companyRating.RejectText = companyRatingData.Rejection?.RejectText;
            companyRating.Commentary = companyRatingData.Commentary;
            companyRating.CreditLimit = companyRatingData.CreditLimit;
            companyRating.Currency = companyRatingData.Currency;
            companyRating.Rating = companyRatingData.Rating;
            companyRating.RatingText = companyRatingData.RatingText;
            companyRating.RiskPrognosis = companyRatingData.RiskPrognosis;
        }

        public async Task<Company> HandleCompanyAsync(string roaringCompanyId, int companyRatingId)
        {
            var existingCompany = _dbContext.Companies.FirstOrDefault(c => c.RoaringCompanyId == roaringCompanyId);

            if (existingCompany == null)
            {
                var newCompany = new Company
                {
                    RoaringCompanyId = roaringCompanyId,
                    CompanyRatingId = companyRatingId
                    // Populate other properties if necessary
                };

                _dbContext.Companies.Add(newCompany);
                await _dbContext.SaveChangesAsync();
                return newCompany;
            }
            else
            {
                if (existingCompany.CompanyRatingId != companyRatingId)
                {
                    existingCompany.CompanyRatingId = companyRatingId;
                    await _dbContext.SaveChangesAsync();
                }
                return existingCompany;
            }
        }

        // Additional mapping methods for other entities can be added here
    }
}
