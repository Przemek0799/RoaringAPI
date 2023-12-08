namespace RoaringAPI.ModelRoaring
{
    public class FinancialRecordApiResponse
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Currency { get; set; }


        public List<FinancialRecordResponse> Records { get; set; }
        public RoaringStatus Status { get; set; }
    }
}