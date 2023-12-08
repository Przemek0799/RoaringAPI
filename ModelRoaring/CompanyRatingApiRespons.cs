namespace RoaringAPI.ModelRoaring
{
    public class CompanyRatingApiRespons
    {
        public string CompanyId { get; set; }
        public int? CreditLimit { get; set; }
        public int? Rating { get; set; }
        public string RatingText { get; set; }
        public string RiskPrognosis { get; set; }
        public string Currency { get; set; }
        public string Commentary { get; set; }

        // Rejection object is nullable to handle cases where it might not be present
        public Rejection Rejection { get; set; }

        public Status Status { get; set; }
    }

    public class Rejection
    {
        public string CauseOfReject { get; set; }
        public string RejectComment { get; set; }
        public string RejectText { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
        public string Text { get; set; }
    }
}
