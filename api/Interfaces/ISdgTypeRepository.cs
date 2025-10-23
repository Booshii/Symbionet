using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ISdgTypeRepository
     {
        Task<List<SdgType>> GetAllAsync(); 
        Task<SdgType?> GetByIdAsync(int id); 
        Task<SdgType> CreateAsync( SdgType sdgModel);
        Task<List<SdgType>> GetByIdsAsync(List<int> sdgTypeIds);
        // Task<SdgTarget> DeleteAsync( int id); 
        // Task<SdgTarget> UpdateAsync(int id, SdgTarget sdgModel); 
    }
}