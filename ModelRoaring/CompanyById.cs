namespace RoaringAPI.ModelRoaring
{
    

    public class RoaringRecord
    {
        public string Address { get; set; }
        public string ChangeDate { get; set; }
        public string CoAddress { get; set; }
        public string Commune { get; set; }
        public string CommuneCode { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistrationDate { get; set; }
        public string County { get; set; }
        public bool EmployerContributionReg { get; set; }
        public string FaxNumber { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryText { get; set; }
        public string LegalGroupCode { get; set; }
        public string LegalGroupText { get; set; }
        public int NumberCompanyUnits { get; set; }
        public string NumberEmployeesInterval { get; set; }
        public bool PreliminaryTaxReg { get; set; }
        public bool SeveralCompanyName { get; set; }
        public string StatusCode { get; set; }
        public string StatusDateFrom { get; set; }
        public string StatusTextDetailed { get; set; }
        public string StatusTextHigh { get; set; }
        public string TopDirectorFunction { get; set; }
        public string TopDirectorName { get; set; }
        public string Town { get; set; }
        public bool VatReg { get; set; }
        public string ZipCode { get; set; }
    }

    public class RoaringStatus
    {
        public int Code { get; set; }
        public string Text { get; set; }
    }

}

