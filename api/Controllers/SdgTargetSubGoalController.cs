using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.SdgTargetDtos;
using api.Dtos.SdgTargetSubGoalDtos;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    /**
     * @brief Controller for managing SDG Target Sub-Goals.
     * 
     * @details This controller provides endpoints for CRUD operations
     * on SDG Target Sub-Goals. It interacts with repositories for data handling
     * and ensures proper validation.
     */
    [Route("api/sdgTargetSubGoal")]
    [ApiController]
    public class SdgTargetSubGoalController : ControllerBase
    {
        private readonly ISdgTargetSubGoalRepository _subgoalRepo; 
        private readonly ISdgTargetRepository _sdgTargetRepo;

        /**
         * @param subgoalRepo Repository for managing SDG Target Sub-Goals.
         * @param sdgTargetRepo Repository for managing SDG Targets.
         */
        public SdgTargetSubGoalController(ISdgTargetSubGoalRepository subgoalRepo, ISdgTargetRepository sdgTargetRepo){
            _subgoalRepo = subgoalRepo;
            _sdgTargetRepo = sdgTargetRepo;
        }
        /**
         * @return IActionResult containing a list of sub-goals as DTOs.
         */
        [HttpGet]
        public async Task<IActionResult> GetAll(){     
            var subgoals = await _subgoalRepo.GetAllAsync(); 
            var subgoalDtos = subgoals.Select(s=>s.ToSdgTargetSubGoalDto());
            return Ok(subgoalDtos); 
        }
        /**
         * @param id The ID of the sub-goal to retrieve.
         * @return IActionResult containing the sub-goal data as a DTO or 404 if not found.
         */
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var subgoal = await _subgoalRepo.GetByIdAsync(id);
            if(subgoal == null){
                return NotFound($"Sub-goal with ID {id} not found."); 
            }
            return Ok(subgoal.ToSdgTargetSubGoalDto());
        }
        /**
         * @param sdgTargetId The ID of the SDG target to associate with the sub-goal.
         * @param createSdgTargetSubGoalDto The data for the new sub-goal.
         * @return IActionResult containing the created sub-goal data as a DTO.
         */
        [HttpPost("{sdgTargetId:int}")]
        public async Task<IActionResult> Create ([FromRoute] int sdgTargetId, CreateSdgTargetSubGoalDto createSdgTargetSubGoalDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            if(!await _sdgTargetRepo.SdgTargetExists(sdgTargetId)){
                return BadRequest($"SDG Target with ID {sdgTargetId} does not exist."); 
            }
            var subgoalModel = createSdgTargetSubGoalDto.ToSdgTargetSubGoalFromCreate(sdgTargetId); 
            await _subgoalRepo.CreateAsync(subgoalModel);

            return CreatedAtAction(nameof(GetById), new {id = subgoalModel.Id}, subgoalModel.ToSdgTargetSubGoalDto());
            
        }
        /**
         * @param id The ID of the sub-goal to update.
         * @param updateSdgTargetSubGoalDto The updated data for the sub-goal.
         * @return IActionResult containing the updated sub-goal data or 404 if not found.
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> Update ([FromRoute] int id, [FromBody] UpdateSdgTargetSubGoalDto updateSdgTargetSubGoalDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
                
            var updatedSubgoal = await _subgoalRepo.UpdateAsync(id, updateSdgTargetSubGoalDto.ToSdgTargetSubGoalFromUpdate()); 
            if(updatedSubgoal == null){
                return NotFound($"Sub-goal with ID {id} not found.");
            }

            return Ok(updatedSubgoal.ToSdgTargetSubGoalDto()); 
        }
        /**
         * @param id The ID of the sub-goal to delete.
         * @return IActionResult with 204 if successful or 404 if not found.
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var deletedSubgoal = await _subgoalRepo.DeleteAsync(id);
            if(deletedSubgoal == null){
                return NotFound($"Sub-goal with ID {id} not found.");
            }

            return NoContent();
        }
        
    }
}
