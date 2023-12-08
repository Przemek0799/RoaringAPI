namespace RoaringAPI.ModelRoaring
{
    public class GroupCompanyResponse
    {
        public string CompanyId { get; set; }
        public int CompanyLevel { get; set; }
        public string CompanyName { get; set; }
        public string CountryCode { get; set; }
        public string MotherCompanyId { get; set; }
        public double OwnedPercentage { get; set; }
    }

}