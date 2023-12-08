// File: Mapping/FinancialRecordMapperService.cs
using Microsoft.EntityFrameworkCore;
using RoaringAPI.Interface;
using RoaringAPI.Model;
using RoaringAPI.ModelRoaring;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoaringAPI.Mapping
{
    public class FinancialRecordMapperService : IFinancialRecordMapperService
    {
        private readonly RoaringDbcontext _dbContext;

        public FinancialRecordMapperService(RoaringDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FinancialRecord> HandleFinancialRecordAsync(FinancialRecordResponse recordResponse, int companyId)
        {
            var existingRecord = _dbContext.FinancialRecords.FirstOrDefault(fr =>
                fr.CompanyId == companyId &&
                fr.FromDate == DateTime.Parse(recordResponse.FromDate) &&
                fr.ToDate == DateTime.Parse(recordResponse.ToDate));

            if (existingRecord != null)
            {
                UpdateExistingFinancialRecord(existingRecord, recordResponse);
            }
            else
            {
                existingRecord = MapFinancialRecord(recordResponse, companyId);
                _dbContext.FinancialRecords.Add(existingRecord);
            }

            await _dbContext.SaveChangesAsync();
            return existingRecord;
        }

        private void UpdateExistingFinancialRecord(FinancialRecord existingRecord, FinancialRecordResponse recordResponse)
        {
            if (existingRecord != null)
            {
                existingRecord.BsShareCapital = decimal.Parse(recordResponse.BsShareCapital.ToString());
                existingRecord.NumberOfShares = recordResponse.NumberOfShares;
                existingRecord.KpiEbitda = decimal.Parse(recordResponse.KpiEbitda.ToString());
                existingRecord.KpiEbitdaMarginPercent = decimal.Parse(recordResponse.KpiEbitdaMarginPercent.ToString());
                existingRecord.KpiReturnOnEquityPercent = decimal.Parse(recordResponse.KpiReturnOnEquityPercent.ToString());
                existingRecord.PlNetOperatingIncome = decimal.Parse(recordResponse.PlNetOperatingIncome.ToString());
                existingRecord.PlSales = decimal.Parse(recordResponse.PlSales.ToString());
                existingRecord.PlEbit = decimal.Parse(recordResponse.PlEbit.ToString());
                existingRecord.PlProfitLossAfterFinItems = decimal.Parse(recordResponse.PlProfitLossAfterFinItems.ToString());
                existingRecord.PlNetProfitLoss = decimal.Parse(recordResponse.PlNetProfitLoss.ToString());
                existingRecord.BsTotalInventories = decimal.Parse(recordResponse.BsTotalInventories.ToString());
                existingRecord.BsCashAndBankBalances = decimal.Parse(recordResponse.BsCashAndBankBalances.ToString());
                existingRecord.BsTotalEquity = decimal.Parse(recordResponse.BsTotalEquity.ToString());
                existingRecord.BsTotalLongTermDebts = decimal.Parse(recordResponse.BsTotalLongTermDebts.ToString());
                existingRecord.BsTotalCurrentLiabilities = decimal.Parse(recordResponse.BsTotalCurrentLiabilities.ToString());
                existingRecord.BsTotalEquityAndLiabilities = decimal.Parse(recordResponse.BsTotalEquityAndLiabilities.ToString());
             

                // Update other fields as needed
            }
           
        }

        private FinancialRecord MapFinancialRecord(FinancialRecordResponse recordResponse, int companyId)
        {
            // Mapping logic here...
            return new FinancialRecord
            {
                CompanyId = companyId, // Assuming this is the foreign key to Company
                FromDate = DateTime.Parse(recordResponse.FromDate),
                ToDate = DateTime.Parse(recordResponse.ToDate),
                BsShareCapital = decimal.Parse(recordResponse.BsShareCapital.ToString()),
                NumberOfShares = recordResponse.NumberOfShares,
                KpiEbitda = decimal.Parse(recordResponse.KpiEbitda.ToString()),
                KpiEbitdaMarginPercent = decimal.Parse(recordResponse.KpiEbitdaMarginPercent.ToString()),
                KpiReturnOnEquityPercent = decimal.Parse(recordResponse.KpiReturnOnEquityPercent.ToString()),
                PlNetOperatingIncome = decimal.Parse(recordResponse.PlNetOperatingIncome.ToString()),
                PlSales = decimal.Parse(recordResponse.PlSales.ToString()),
                PlEbit = decimal.Parse(recordResponse.PlEbit.ToString()),
                PlProfitLossAfterFinItems = decimal.Parse(recordResponse.PlProfitLossAfterFinItems.ToString()),
                PlNetProfitLoss = decimal.Parse(recordResponse.PlNetProfitLoss.ToString()),
                BsTotalInventories = decimal.Parse(recordResponse.BsTotalInventories.ToString()),
                BsCashAndBankBalances = decimal.Parse(recordResponse.BsCashAndBankBalances.ToString()),
                BsTotalEquity = decimal.Parse(recordResponse.BsTotalEquity.ToString()),
                BsTotalLongTermDebts = decimal.Parse(recordResponse.BsTotalLongTermDebts.ToString()),
                BsTotalCurrentLiabilities = decimal.Parse(recordResponse.BsTotalCurrentLiabilities.ToString()),
                BsTotalEquityAndLiabilities = decimal.Parse(recordResponse.BsTotalEquityAndLiabilities.ToString()),
               
            };
        }

        public async Task<Company> HandleCompanyAsync(string roaringCompanyId)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.RoaringCompanyId == roaringCompanyId);

            if (company == null)
            {
                company = new Company
                {
                    RoaringCompanyId = roaringCompanyId,
                    // Populate other fields if necessary
                };

                _dbContext.Companies.Add(company);
                await _dbContext.SaveChangesAsync();
            }

            return company;
        }
    }
}
