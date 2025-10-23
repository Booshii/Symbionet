using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Company
{
    public class CreateCompanyDto
    {
        
        [Required]
        [MaxLength(100, ErrorMessage = "Title is too long")]
        public string Name { get; set; } = string.Empty; 
        public string Street { get; set; } = string.Empty; 
        public string StreetNumber { get; set; } = string.Empty; 
        public int Postalcode { get; set; }
        public string City { get; set; } = string.Empty; 
        public string ContactPerson { get; set; } = string.Empty; 
        public string ContactEmail { get; set; } = string.Empty; 
        public string ContactTel { get; set; } = string.Empty; 
        public string Latitude { get; set; } = string.Empty; 
        public string Longitude { get; set; } = string.Empty; 
        public string WebsiteLink { get; set; } = string.Empty; 
   
    }
}