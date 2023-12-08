using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoaringAPI.Model
{
    public class CompanyEmployee
    {
        [Key]
        public int EmployeeInCompanyId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public string? TopDirectorFunction { get; set; }
        public string? TopDirectorName { get; set; }

       

    }
}
