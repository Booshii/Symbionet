using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ISdgTargetSubGoalRepository
    {
        Task<List<SdgTargetSubGoal>> GetAllAsync(); 
        Task<SdgTargetSubGoal?> GetByIdAsync(int id); 
        Task<SdgTargetSubGoal> CreateAsync( SdgTargetSubGoal sdgTargetSubGoal); 
        Task<SdgTargetSubGoal> DeleteAsync( int id); 
        Task<SdgTargetSubGoal> UpdateAsync(int id, SdgTargetSubGoal sdgTargetSubGoal);
    }
}