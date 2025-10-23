using api.Dtos.Industry;
using api.Models;

namespace api.Interfaces
{
    public interface IIndustryRepository
    {
        Task<List<Industry>> GetAllAsync();
        Task<Industry> CreateAsync(Industry industryModel);
        Task<Industry?> UpdateAsync(int id, UpdateIndustryRequestDto updateIndustry);
        Task<Industry?> GetByIdAsync(int id);
        Task<Industry?> DeleteAsync(int id);
        Task<bool> IndustryExists(int id); 
    }
}