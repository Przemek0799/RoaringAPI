using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoaringAPI.Model
{
    public class CompanyRating
    {

        [Key]

        public int CompanyRatingId { get; set; }

        public string? CauseOfReject { get; set; } 
        public string? RejectComment { get; set; } 
        public string? RejectText { get; set; } 

        public string? Commentary { get; set; }
        public int? CreditLimit { get; set; }
        public string? Currency { get; set; }
        public int? Rating { get; set; }
        public string? RatingText { get; set; }
        public string? RiskPrognosis { get; set; }
    }
}



  