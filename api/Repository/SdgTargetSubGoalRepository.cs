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
    public class SdgTargetSubGoalRepository : ISdgTargetSubGoalRepository
    {
        private readonly ApplicationDBContext _context; 

        public SdgTargetSubGoalRepository(ApplicationDBContext context)
        {
            _context = context; 
        }
        public async Task<SdgTargetSubGoal> CreateAsync(SdgTargetSubGoal sdgTargetSubGoalModel)
        {
            await _context.SdgTargetSubGoals.AddAsync(sdgTargetSubGoalModel);
            await _context.SaveChangesAsync();

            return sdgTargetSubGoalModel;
        }

        public async Task<SdgTargetSubGoal> DeleteAsync(int id)
        {
            var subgoalModel = await _context.SdgTargetSubGoals.FirstOrDefaultAsync(x => x.Id == id); 
            if(subgoalModel == null){
                    return null; 
            }
            _context.SdgTargetSubGoals.Remove(subgoalModel); 
            await _context.SaveChangesAsync(); 
            return subgoalModel; 
        }

        public async Task<List<SdgTargetSubGoal>> GetAllAsync()
        {
            return await _context.SdgTargetSubGoals.ToListAsync(); 
        }

        public async Task<SdgTargetSubGoal?> GetByIdAsync(int id)
        {
            return await _context.SdgTargetSubGoals.FindAsync(id); 
        }

        public async Task<SdgTargetSubGoal> UpdateAsync(int id, SdgTargetSubGoal subgoalModel)
        {
            var existingSubGoal = await _context.SdgTargetSubGoals.FindAsync(id); 

            if(existingSubGoal == null){
                return null; 
            }

            existingSubGoal.Title = subgoalModel.Title;
            existingSubGoal.Description = subgoalModel.Description;

            await _context.SaveChangesAsync(); 

            return existingSubGoal; 
        }

        
    }
}