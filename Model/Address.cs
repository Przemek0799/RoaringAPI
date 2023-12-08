using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoaringAPI.Model
{
    public class Address
    {
        public int AddressId { get; set; } // Primärnyckel
        public int CompanyId { get; set; } // Främmande nyckel

        public string? AddressLine { get; set; } 
        public string? CoAddress { get; set; }
        public string? Commune { get; set; }
        public int? CommuneCode { get; set; }
        public string? County { get; set; }
        public int? ZipCode { get; set; }
        public string? Town { get; set; }

        
      
    }
}
