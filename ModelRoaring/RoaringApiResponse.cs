using Newtonsoft.Json;
using System;

namespace RoaringAPI.ModelRoaring
{
    public class RoaringApiResponse
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<GroupCompanyResponse> GroupCompanies { get; set; }

        public List<RoaringRecord> Records { get; set; }

        public RoaringStatus Status { get; set; }
    }
}




