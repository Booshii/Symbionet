using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Company;
using api.Models;

namespace api.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllAsync(); 
        Task<Company?> GetByIdAsync(int id); 
        Task<Company> CreateAsync(Company companyModel); 
        Task<Company?> UpdateAsync(int id, UpdateCompanyRequestDto updateCompanyRequestDto); 
        Task<Company?> DeleteAsync(int id); 
        Task<bool> CompanyExists(int id); 
    }
}