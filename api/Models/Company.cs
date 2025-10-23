using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    // The 'Table' attribute specifies the database table that this class maps to.
    // 'Companies' is the name of the table in the database.
    [Table("Companies")]
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Street { get; set; } = string.Empty; 
        public string StreetNumber { get; set; } = string.Empty; 
        public int Postalcode { get; set; }
        public string City { get; set; } = string.Empty; 
        public string Latitude { get; set; } = string.Empty; 
        public string Longitude { get; set; } = string.Empty; 

        public string ContactPerson { get; set; } = string.Empty; 
        public string ContactEmail { get; set; } = string.Empty; 
        public string ContactTel { get; set; } = string.Empty; 
        // 'SdgTargets' property (one to many)
        // reicht das aus als Verbindung oder muss da noch mehr für die Datenbank 
        public int IndustryId { get; set; }
        public string WebsiteLink  { get; set; } = string.Empty;
        

        
        // Navigation Properties
        public Industry? Industry { get; set; }
        public List<SdgTarget> SdgTargets { get; set; } = new List<SdgTarget>();
        // 'AppUsers' property (one to many)
        public List<AppUser> AppUsers { get; set; } = new List<AppUser>();
    }
}