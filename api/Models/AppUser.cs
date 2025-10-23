using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    // IdentityUser added default stuff for a user 
    // password , password conjuntation etc 
    public class AppUser : IdentityUser
    {
        // here you can add some specific Things u want 
        // Like Biometrics or Userspecific things 
        public string Name { get; set; } = string.Empty; 
        public int? CompanyId { get; set; }

        // Navigation Properties
        public Company? Company { get; set; }
    }
}