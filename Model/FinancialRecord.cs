using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoaringAPI.Model
{
    public class FinancialRecord
    {
        [Key]
        public int FinancialRecordID { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public decimal BsShareCapital { get; set; }
        public int NumberOfShares { get; set; }

        // KPIs
        public decimal KpiEbitda { get; set; }
        public decimal KpiEbitdaMarginPercent { get; set; }
        public decimal KpiReturnOnEquityPercent { get; set; }
  

        // Profit and Loss (P&L)
        public decimal PlNetOperatingIncome { get; set; }
        public decimal PlSales { get; set; }
        public decimal PlEbit { get; set; }
        public decimal PlProfitLossAfterFinItems { get; set; }
        public decimal PlNetProfitLoss { get; set; }

        // Balance Sheet (BS)
        public decimal BsTotalInventories { get; set; }
        public decimal BsCashAndBankBalances { get; set; }
        public decimal BsTotalEquity { get; set; }
        public decimal BsTotalLongTermDebts { get; set; }
        public decimal BsTotalCurrentLiabilities { get; set; }
        public decimal BsTotalEquityAndLiabilities { get; set; }

    }
}
