using api.Data;
using api.Interfaces;
using api.Dtos.Industry;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class IndustryRepository : IIndustryRepository
    {
        private readonly ApplicationDBContext _context;

        public IndustryRepository(ApplicationDBContext context){
            _context = context; 
        }
        public async Task<Industry> CreateAsync(Industry industryModel)
        {
            await _context.Industries.AddAsync(industryModel); 
            await _context.SaveChangesAsync(); 
            return industryModel;
        }

        public async Task<List<Industry>> GetAllAsync()
        {
            return await _context.Industries.ToListAsync(); 
        }
        public async Task<Industry?> GetByIdAsync(int id)
        {
            return await _context.Industries.FindAsync(id);
        }

        public async Task<Industry?> UpdateAsync(int id, UpdateIndustryRequestDto updateIndustry)
        {
            var existingIndustry = await _context.Industries.FindAsync(id);

            if(existingIndustry == null){
                return null;
            }

            existingIndustry.Name = updateIndustry.Name; 

            await _context.SaveChangesAsync(); 
            return existingIndustry; 
        }

        public async Task<Industry?> DeleteAsync(int id)
        {
            var industryModel = await _context.Industries.FirstOrDefaultAsync( i => i.Id == id );
            if(industryModel == null){
                return null;
            }

            _context.Industries.Remove(industryModel);
            await _context.SaveChangesAsync();
            return industryModel; 
        }
        public async Task<bool> IndustryExists(int id){
            return await _context.Industries.AnyAsync(industry => industry.Id == id);
        }
                        
    }
}