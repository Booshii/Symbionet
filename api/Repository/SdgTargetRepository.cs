using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Data;
using api.Helpers;
using api.Interfaces; 
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Repository
{
    public class SdgTargetRepository : ISdgTargetRepository
    {
        // Field to hold the database context used for data operations.
        private readonly ApplicationDBContext _context; 

        // Constructor that injects the database context into the repository.
        // This allows the repository to perform data operations.
        public SdgTargetRepository(ApplicationDBContext context)
        {
            _context = context; 
        }
    
        public async Task<List<SdgTarget>> GetAllAsync(QueryObject query){

            // Create a query that has not been executed yet
            var sdgTargets = _context.SdgTargets
                .Include(st => st.Company)
                .Include(st => st.SubGoals)
                .Include(st => st.SdgTargetSdgTypes)
                    .ThenInclude(stst => stst.SdgType)
                .AsQueryable(); 
            // Add a filter if CompanyId is set
            if (query.CompanyId.HasValue){
                sdgTargets = sdgTargets.Where(s=>s.CompanyId.Value == query.CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy)){
                
                if(query.SortBy.Equals("IsDone", StringComparison.OrdinalIgnoreCase)){
                    sdgTargets = query.IsDecsending 
                        ? sdgTargets.OrderByDescending(s=>s.IsDone).ThenByDescending(s => s.CreationDate) 
                        : sdgTargets.OrderByDescending(s => s.IsDone).ThenByDescending(s => s.CreationDate); 
                }
            }
            // Execute the query and fetch the filtered results
            return await sdgTargets.ToListAsync();
        }

        public async Task<SdgTarget?> GetByIdAsync(int id)
        {
            return await _context.SdgTargets
            .Include(sg => sg.SubGoals)
            .Include(st => st.SdgTargetSdgTypes)
                .ThenInclude(stst => stst.SdgType)
            .FirstOrDefaultAsync(st => st.Id == id);
        }
        public async Task<SdgTarget> CreateAsync(SdgTarget sdgTargetModel){
            //adds a new SDG target to the database.
            await _context.SdgTargets.AddAsync(sdgTargetModel); 
            // After adding, it saves the changes to the database. (necessary)
            await _context.SaveChangesAsync(); 
            // Returns the newly created SDG target.
            return sdgTargetModel; 

        }

        public async Task<SdgTarget?> DeleteAsync(int id)
        {
            var SdgTargetModel = await _context.SdgTargets.FirstOrDefaultAsync(x => x.Id == id);
            if(SdgTargetModel == null){
                return null; 
            }
            _context.SdgTargets.Remove(SdgTargetModel); 
            await _context.SaveChangesAsync(); 
            return SdgTargetModel;
        }
        public async Task<SdgTarget?> UpdateAsync(int id, SdgTarget updatedSdgTarget){
            var existingSdgTarget = await _context.SdgTargets
                .Include(st => st.SdgTargetSdgTypes)
                .FirstOrDefaultAsync(st => st.Id == id); 

            if(existingSdgTarget == null){
                return null;
            }

            _context.Entry(existingSdgTarget).CurrentValues.SetValues(updatedSdgTarget);

            existingSdgTarget.SdgTargetSdgTypes = updatedSdgTarget.SdgTargetSdgTypes
                .Select(sdgType => new SdgTargetSdgType{
                    SdgTargetId = existingSdgTarget.Id,
                    SdgTypeId = sdgType.SdgTypeId
                }).ToList();

            await _context.SaveChangesAsync(); 
            
            return existingSdgTarget; 
        }  

        public Task<bool> SdgTargetExists(int id)
        {
            return _context.SdgTargets.AnyAsync(s => s.Id == id); 
        }

    }
}