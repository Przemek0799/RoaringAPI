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

        public async Task<CompanyRating> CreateOrUpdateCompanyRatingAsync(CompanyRatingApiRespons companyRatingData)
        {
            if (companyRatingData == null)
                return null;

            // Handle potential nulls in rejection properties
            var causeOfReject = companyRatingData.Rejection?.CauseOfReject ?? string.Empty;
            var rejectComment = companyRatingData.Rejection?.RejectComment ?? string.Empty;
            var rejectText = companyRatingData.Rejection?.RejectText ?? string.Empty;

            // Check if there is already an entry with the same data
            var existingCompanyRating = _dbContext.CompanyRatings
                .AsNoTracking()
                .FirstOrDefault(cr =>
                    cr.Rating == companyRatingData.Rating &&
                    cr.CreditLimit == companyRatingData.CreditLimit &&
                    cr.Currency == companyRatingData.Currency &&
                    cr.CauseOfReject == causeOfReject &&
                    cr.RejectComment == rejectComment &&
                    cr.RejectText == rejectText &&
                    cr.Commentary == companyRatingData.Commentary &&
                    cr.RatingText == companyRatingData.RatingText &&
                    cr.RiskPrognosis == companyRatingData.RiskPrognosis);

            if (existingCompanyRating == null)
            {
                var newCompanyRating = MapCompanyRating(companyRatingData);
                _dbContext.CompanyRatings.Add(newCompanyRating);
                await _dbContext.SaveChangesAsync();
                return newCompanyRating;
            }

            // If an existing entry matches, return it without making changes
            return existingCompanyRating;
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
