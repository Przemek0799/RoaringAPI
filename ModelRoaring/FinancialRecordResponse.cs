namespace RoaringAPI.ModelRoaring
{
    public class FinancialRecordResponse
    {
        public string CompanyId { get; set; }
        public string ChangeDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Currency { get; set; }
        public int BsShareCapital { get; set; }
        public int NumberOfShares { get; set; }
        public int NumberOfEmployees { get; set; }
        public double KpiEbitda { get; set; }
        public double KpiEbitdaMarginPercent { get; set; }
        public double KpiReturnOnEquityPercent { get; set; }
        public double KpiReturnOnCapitalEmployedPercent { get; set; }
        public double KpiEquityRatioPercent { get; set; }
        public double KpiQuickRatioPercent { get; set; }
        public double PlNetOperatingIncome { get; set; }
        public double PlSales { get; set; }
        public double PlEbit { get; set; }
        public double PlProfitLossAfterFinItems { get; set; }
        public double PlNetProfitLoss { get; set; }
        public int BsTotalInventories { get; set; }
        public int BsCashAndBankBalances { get; set; }
        public int BsTotalEquity { get; set; }
        public int BsTotalLongTermDebts { get; set; }
        public int BsTotalCurrentLiabilities { get; set; }
        public int BsTotalEquityAndLiabilities { get; set; }
        public string ParentGroupFinancialInformation { get; set; }

    }
   
}
