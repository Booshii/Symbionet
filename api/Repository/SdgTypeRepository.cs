using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class SdgTypeRepository : ISdgTypeRepository
    {
        // Field to hold the database context used for data operations.
        private readonly ApplicationDBContext _context; 

        // Constructor that injects the database context into the repository.
        // This allows the repository to perform data operations.
        public SdgTypeRepository(ApplicationDBContext context)
        {
            _context = context; 
        }
    
        public Task<SdgType> CreateAsync(SdgType sdgModel)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SdgType>> GetAllAsync()
        {
            return await _context.SdgTypes.ToListAsync(); 
        }

        public async Task<SdgType?> GetByIdAsync(int id)
        {
            return await _context.SdgTypes.FindAsync(id);        
        }

        public async Task<List<SdgType>> GetByIdsAsync(List<int> sdgTypeIds)
        {
             if (sdgTypeIds == null || !sdgTypeIds.Any()){
                return new List<SdgType>(); 
            }

    var allSdgTypes = await _context.SdgTypes.ToListAsync(); // ✅ await notwendig!
    return allSdgTypes.Where(s => sdgTypeIds.Contains(s.Id)).ToList(); 
}
    }
}