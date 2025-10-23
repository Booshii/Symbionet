using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class SdgTargetSdgTypeRepository : ISdgTargetSdgTypeRepository
    {
        private readonly ApplicationDBContext _context; 
        public SdgTargetSdgTypeRepository(ApplicationDBContext context)
        {
            _context = context; 
        }
    
        public async Task<SdgTargetSdgType> CreateAsync(SdgTargetSdgType sdgTargetSdgType)
        {
            await _context.SdgTargetSdgTypes.AddAsync(sdgTargetSdgType);
            await _context.SaveChangesAsync(); 

            return sdgTargetSdgType;
        }
    }
}