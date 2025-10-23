using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Account
{
  public class AppUserDto
  {
    public string Id { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }             
    public string? PhoneNumber { get; set; }  
    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? Role { get; set; } = string.Empty; 
    public string Token { get; set; } = string.Empty; 


  }
}