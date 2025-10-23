using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    /**
     * @brief Controller for managing SDG Types.
     * 
     * @details This controller provides endpoints to retrieve all SDG types
     * and their simplified representations.
     */
    [Route("api/sdgtype")]
    public class SdgTypeController : ControllerBase
    {
        private readonly ISdgTypeRepository _sdgTypeRepo; 
        
        /**
         * @param sdgTypeRepo Repository for managing SDG types.
         */
        public SdgTypeController(ISdgTypeRepository sdgTypeRepo){
            _sdgTypeRepo = sdgTypeRepo;
        }
        /**
         * @return IActionResult containing a list of SDG types as DTOs.
         */
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var sdgTypes = await _sdgTypeRepo.GetAllAsync(); 
            if (sdgTypes == null || !sdgTypes.Any()){
                return NotFound("No SDG types found.");
            }
            
            var sdgTypesDto = sdgTypes.Select(s => s.ToSdgTypeDto());
            return Ok(sdgTypesDto); 
        }
        /**
         * @return IActionResult containing a list of simplified SDG types as DTOs.
         */
        [HttpGet("simple")]
        public async Task<IActionResult> GetAllSimple(){
            var sdgTypes = await _sdgTypeRepo.GetAllAsync(); 
            if (sdgTypes == null || !sdgTypes.Any()){
                return NotFound("No SDG types found.");
            }

            var simpleSdgTypes = sdgTypes.Select(s => s.ToSimpleSdgTypeDto());
            return Ok(simpleSdgTypes); 
        }
    }
}