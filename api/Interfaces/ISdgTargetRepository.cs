using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using api.Models; 

namespace api.Interfaces
{
    public interface ISdgTargetRepository
    {
        Task<List<SdgTarget>> GetAllAsync(QueryObject query); 
        Task<SdgTarget?> GetByIdAsync(int id); 
        Task<SdgTarget> CreateAsync( SdgTarget sdgModel); 
        Task<SdgTarget?> DeleteAsync( int id); 
        Task<SdgTarget?> UpdateAsync(int id, SdgTarget sdgModel); 
        Task<bool> SdgTargetExists(int id); 
    }
}