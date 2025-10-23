using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Company;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        // Field to hold the database context used for data operations.
        private readonly ApplicationDBContext _context; 

        // Constructor that injects the database context into the repository.
        // This allows the repository to perform data operations.
        public CompanyRepository(ApplicationDBContext context){
            _context = context; 
        }
        
        public async Task<Company> CreateAsync(Company companyModel)
        {
            //adds a new company to the database.            
            await _context.Companies.AddAsync(companyModel); 
            // After adding, it saves the changes to the database. (necessary)
            await _context.SaveChangesAsync(); 
            
            // Returns the newly created company with linked industry again
            var createdCompany = await _context.Companies
                .Include(c => c.Industry)
                .FirstOrDefaultAsync(c => c.Id == companyModel.Id);

            return createdCompany; 
        }

        public async Task<Company?> DeleteAsync(int id)
        {
            var companyModel = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id ); 
            if (companyModel == null){
                return null; 
            }
            _context.Companies.Remove(companyModel); 
            await _context.SaveChangesAsync(); 
            return companyModel; 
        }

        public async Task<List<Company>> GetAllAsync()
        {   
            // Versuch einen Filter zu bauen - noch mal gucken Query Objekt muss dann als Parameter übergeben werden 
            // var companies = _context.Companies.Include(c => c.Comments).AsQueryable();
            // if(!string.IsNullOrWhiteSpace(query.CompanyName)){
            //     companies = companies.Where(c => c.CompanyName);
            //

            var Companies =  _context.Companies
                .Include(c => c.SdgTargets)
                .ThenInclude(target => target.SdgTargetSdgTypes)
                .ThenInclude(sdgTargetSdgType => sdgTargetSdgType.SdgType)
                .Include(st => st.Industry);



            return await Companies.ToListAsync(); 

            
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            var Company = await _context.Companies
                .Include(c => c.SdgTargets)
                    .ThenInclude(target => target.SdgTargetSdgTypes)
                    .ThenInclude(sdgTargetSdgType => sdgTargetSdgType.SdgType)
                .Include(c => c.Industry)
                .FirstOrDefaultAsync(c => c.Id == id);

            return Company; 
        }

        public async Task<Company?> UpdateAsync(int id, UpdateCompanyRequestDto updateCompanyRequestDto)
        {
            var existingCompany = await _context.Companies.FindAsync(id); 

            if(existingCompany == null){
                return null; 
            }

            existingCompany.Name = updateCompanyRequestDto.Name; 

            // Aktuallisierung der Adresse
            existingCompany.Street = updateCompanyRequestDto.Street;
            existingCompany.StreetNumber = updateCompanyRequestDto.StreetNumber;
            existingCompany.Postalcode = updateCompanyRequestDto.Postalcode; 
            existingCompany.City = updateCompanyRequestDto.City;
            existingCompany.IndustryId = updateCompanyRequestDto.IndustryId;
            existingCompany.ContactPerson = updateCompanyRequestDto.ContactPerson;
            existingCompany.ContactEmail = updateCompanyRequestDto.ContactEmail;
            existingCompany.ContactTel = updateCompanyRequestDto.ContactTel; 
            existingCompany.Latitude = updateCompanyRequestDto.Latitude;
            existingCompany.Longitude = updateCompanyRequestDto.Longitude;
            existingCompany.WebsiteLink = updateCompanyRequestDto.WebsiteLink; 

            await _context.SaveChangesAsync(); 
            return existingCompany;
        }
        public async Task<bool> CompanyExists(int id){
            return await _context.Companies.AnyAsync(company => company.Id == id);
        }
    }
}