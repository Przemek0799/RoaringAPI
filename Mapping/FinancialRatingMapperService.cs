using Microsoft.EntityFrameworkCore;
using RoaringAPI.ControllersRoaring;
using RoaringAPI.Interface;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;

namespace RoaringAPI.Mapping
{
    public class FinancialRatingMapperService : IFinancialRatingMapperService
    {
        private readonly RoaringDbcontext _dbContext;
        private readonly ILogger<FinancialRatingMapperService> _logger;


        public FinancialRatingMapperService(RoaringDbcontext dbContext, ILogger<FinancialRatingMapperService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

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
            {
                _logger.LogInformation("No company rating data provided for companyId {CompanyId}", roaringCompanyId);
                return null;
            }

            // Normalize nulls to empty strings for comparison
            var causeOfRejectNormalized = companyRatingData.Rejection?.CauseOfReject ?? string.Empty;
            var rejectCommentNormalized = companyRatingData.Rejection?.RejectComment ?? string.Empty;
            var rejectTextNormalized = companyRatingData.Rejection?.RejectText ?? string.Empty;
            var commentaryNormalized = companyRatingData.Commentary ?? string.Empty;
            var ratingTextNormalized = companyRatingData.RatingText ?? string.Empty;
            var riskPrognosisNormalized = companyRatingData.RiskPrognosis ?? string.Empty;

            // Log the fetched data and comparison parameters
            _logger.LogInformation("Fetched data for companyId {CompanyId}: {CompanyRatingData}", roaringCompanyId, companyRatingData);
            _logger.LogInformation("Checking for existing rating with Rating: {Rating}, CreditLimit: {CreditLimit}, Currency: {Currency}, CauseOfReject: {CauseOfReject}, RejectComment: {RejectComment}, RejectText: {RejectText}, Commentary: {Commentary}, RatingText: {RatingText}, RiskPrognosis: {RiskPrognosis}",
                companyRatingData.Rating, companyRatingData.CreditLimit, companyRatingData.Currency, causeOfRejectNormalized, commentaryNormalized, rejectTextNormalized, companyRatingData.Commentary, companyRatingData.RatingText, companyRatingData.RiskPrognosis);
            // Check if there is already an entry with the same data
            var existingCompanyRating = await _dbContext.CompanyRatings
                .AsNoTracking()
                .FirstOrDefaultAsync(cr =>
                    cr.Rating == companyRatingData.Rating &&
                    cr.CreditLimit == companyRatingData.CreditLimit &&
                    cr.Currency == companyRatingData.Currency &&
                    (cr.CauseOfReject == causeOfRejectNormalized || (cr.CauseOfReject == null && string.IsNullOrEmpty(causeOfRejectNormalized))) &&
                    (cr.RejectComment == rejectCommentNormalized || (cr.RejectComment == null && string.IsNullOrEmpty(rejectCommentNormalized))) &&
                    (cr.RejectText == rejectTextNormalized || (cr.RejectText == null && string.IsNullOrEmpty(rejectTextNormalized))) &&
                    (cr.Commentary == commentaryNormalized || (cr.Commentary == null && string.IsNullOrEmpty(commentaryNormalized))) &&
                    (cr.RatingText == ratingTextNormalized || (cr.RatingText == null && string.IsNullOrEmpty(ratingTextNormalized))) &&
                    (cr.RiskPrognosis == riskPrognosisNormalized || (cr.RiskPrognosis == null && string.IsNullOrEmpty(riskPrognosisNormalized))));

            if (existingCompanyRating != null)
            {
                // Log found existing rating
                _logger.LogInformation("Existing rating found for companyId {CompanyId}: {ExistingRating}", roaringCompanyId, existingCompanyRating);

                // If an existing entry matches, use it without making changes
                await HandleCompanyAsync(roaringCompanyId, existingCompanyRating.CompanyRatingId);
                return existingCompanyRating;
            }
            else
            {
                _logger.LogInformation("No existing rating found for companyId {CompanyId}, creating new rating.", roaringCompanyId);

                // Create new rating and company
                var newCompanyRating = MapCompanyRating(companyRatingData);
                _dbContext.CompanyRatings.Add(newCompanyRating);
                await _dbContext.SaveChangesAsync();

                // Log creation of new rating
                _logger.LogInformation("Created new rating for companyId {CompanyId}: {NewRating}", roaringCompanyId, newCompanyRating);

                await HandleCompanyAsync(roaringCompanyId, newCompanyRating.CompanyRatingId);
                return newCompanyRating;
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
