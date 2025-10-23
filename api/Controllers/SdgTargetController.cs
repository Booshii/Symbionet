using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Dtos.SdgTargetDtos;
using api.Dtos.SdgTargetSubGoalDtos;
using api.Helpers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace api.Controllers 
{   
    /**
     * @brief Controller for managing SDG Targets.
     * 
     * @details This controller provides endpoints for CRUD operations on SDG targets.
     * It uses repositories to interact with the database and ensures proper validation.
     */
    [Route("api/sdg")]
    [ApiController]
    public class SdgTargetController : ControllerBase{

        private readonly ISdgTargetRepository _sdgTargetRepo; 
        private readonly ISdgTargetSubGoalRepository _subgoalRepo;
        private readonly ISdgTargetSdgTypeRepository _sdgTargetSdgTypeRepo; 
        private readonly ISdgTypeRepository _sdgTypeRepo; 
        private readonly ICompanyRepository _companyRepo; 

        /**
         * @param sdgTargetRepo Repository for managing SDG targets.
         * @param subgoalRepo Repository for managing SDG target sub-goals.
         * @param sdgTargetSdgTypeRepo Repository for mapping SDG types to targets.
         * @param sdgTypeRepo Repository for managing SDG types.
         * @param companyRepo Repository for managing companies.
         */
        public SdgTargetController(ISdgTargetRepository sdgTargetRepo, ISdgTargetSubGoalRepository subgoalRepo, 
                ISdgTargetSdgTypeRepository sdgTargetSdgTypeRepo, ISdgTypeRepository sdgTypeRepo, ICompanyRepository companyRepo){
            _sdgTypeRepo = sdgTypeRepo;
            _sdgTargetSdgTypeRepo = sdgTargetSdgTypeRepo;
            _sdgTargetRepo = sdgTargetRepo; 
            _subgoalRepo = subgoalRepo; 
            _companyRepo = companyRepo; 
        }
        /**
         * @param query Query parameters for filtering and pagination.
         * @return IActionResult containing a list of SDG targets as DTOs.
         */
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var sdgTargetsFromDB = await _sdgTargetRepo.GetAllAsync(query);  
            if(sdgTargetsFromDB ==null){
              return NotFound("No SDG targets found."); 
            }

            var SdgTargetDtos = sdgTargetsFromDB.Select(s => s.ToSdgTargetDto());
            return Ok(SdgTargetDtos); 
        }
        /**
         * @param id The ID of the SDG target.
         * @return IActionResult containing the SDG target data as a DTO or 404 if not found.
         */
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var sdgTargetFromDB = await _sdgTargetRepo.GetByIdAsync(id);
            if(sdgTargetFromDB == null){
                // gucken ob das so funktioniert sonst wie bei PostCompany
              return NotFound($"SDG Target with ID {id} not found."); 
            }
            
            var sdgTargetDto = sdgTargetFromDB.ToSdgTargetDto();
            return Ok(sdgTargetDto); 
        }
        /**
         * @param companyId The ID of the company the target belongs to.
         * @param sdgTargetDto The data for the new SDG target.
         * @return IActionResult with the created SDG target data or an error message.
         */
        [HttpPost("{companyId:int}")]
        public async Task<IActionResult> Create([FromRoute] int companyId, [FromBody] CreateSdgTargetDto sdgTargetDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            // save user who created this sdgTarget
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) {
                return Unauthorized("No user ID found in token.");
            }

            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
            {
                return BadRequest("Company does not exist");
            }

            var sdgTargetModel = sdgTargetDto.ToSdgTargetFromCreate(companyId);
            sdgTargetModel.CreatedByUserId = userId;
            sdgTargetModel.Company = company; 

            await _sdgTargetRepo.CreateAsync(sdgTargetModel); 
        
            // add sub-goals
            if (sdgTargetDto.SubGoals != null && sdgTargetDto.SubGoals.Any())
            {
                foreach (CreateSdgTargetSubGoalDto subGoalDto in sdgTargetDto.SubGoals)
                {
                    var subgoalModel = subGoalDto.ToSdgTargetSubGoalFromCreate(sdgTargetModel.Id);
                    await _subgoalRepo.CreateAsync(subgoalModel);
                    sdgTargetModel.SubGoals.Add(subgoalModel);
                }
            }
            // Add SDG types
            if (sdgTargetDto.SdgTypeIds != null && sdgTargetDto.SdgTypeIds.Any()){

                var sdgTypes = await _sdgTypeRepo.GetByIdsAsync(sdgTargetDto.SdgTypeIds);
                
                if (sdgTypes.Count != sdgTargetDto.SdgTypeIds.Count){
                    return BadRequest("Some SDG types were not found.");
                }
                 
                foreach (var sdgType in sdgTypes)
                {                       
                    var sdgTargetSdgTypeModel = new SdgTargetSdgType
                    {
                        // hier kann was fehlen 
                        // SdgTargetId = sdgTargetModel.Id,
                        // SdgTypeId = sdgTypeId,
                        SdgTarget = sdgTargetModel,
                        SdgType = sdgType
                    };

                    await _sdgTargetSdgTypeRepo.CreateAsync(sdgTargetSdgTypeModel); 
                    sdgTargetModel.SdgTargetSdgTypes.Add(sdgTargetSdgTypeModel); 
                }   
            }
            // Nachladen des vollständigen SdgTargets mit `Company`
            sdgTargetModel = await _sdgTargetRepo.GetByIdAsync(sdgTargetModel.Id);

            return CreatedAtAction(nameof(GetById), new { id = sdgTargetModel.Id}, sdgTargetModel.ToSdgTargetDto());  
        }


[HttpPost("fileUpload")]
public async Task<IActionResult> UploadJson([FromBody] List<CreateSdgTargetFileUploadDto> jsonData)
{
    try
    {
        if (jsonData == null || jsonData.Count == 0)
        {
            return BadRequest("Keine Datei hochgeladen");
        }

        List<SdgTargetDto> savedSdgTargets = new List<SdgTargetDto>();

        foreach (var company in jsonData)
        {
            if (company.CompanyId <= 0 || company.CreateSdgTargetDtos == null)
            {
                return BadRequest("Fehlende Unternehmens- oder SDG-Daten.");
            }

            foreach (var createSdgTargetDto in company.CreateSdgTargetDtos)
            {
                if (string.IsNullOrEmpty(createSdgTargetDto.Title) ||
                    string.IsNullOrEmpty(createSdgTargetDto.Description) ||
                    createSdgTargetDto.SdgTypeIds == null)
                {
                    return BadRequest("Fehlende oder falsche SDG-Maßnahmendaten.");
                }

                var sdgTargetModel = createSdgTargetDto.ToSdgTargetFromCreate(company.CompanyId);


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) {
                    return Unauthorized("No user ID found in token.");
                }

                sdgTargetModel.CreatedByUserId = userId;
  
                await _sdgTargetRepo.CreateAsync(sdgTargetModel);

                // Add SDG types
                if (createSdgTargetDto.SdgTypeIds.Any())
                {
                    var sdgTypes = await _sdgTypeRepo.GetByIdsAsync(createSdgTargetDto.SdgTypeIds);
                    if (sdgTypes.Count != createSdgTargetDto.SdgTypeIds.Count)
                    {
                        return BadRequest("Some SDG types were not found.");
                    }

                    foreach (var sdgType in sdgTypes)
                    {
                        var sdgTargetSdgTypeModel = new SdgTargetSdgType
                        {
                            SdgTarget = sdgTargetModel,
                            SdgType = sdgType
                        };

                        await _sdgTargetSdgTypeRepo.CreateAsync(sdgTargetSdgTypeModel);
                        sdgTargetModel.SdgTargetSdgTypes.Add(sdgTargetSdgTypeModel);
                    }
                }

                // Speichere das erfolgreich verarbeitete SDG-Target
                savedSdgTargets.Add(sdgTargetModel.ToSdgTargetDto());
            }
        }

        return Ok(new { message = "Daten erfolgreich empfangen", savedData = savedSdgTargets });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Fehler beim Verarbeiten der Datei: {ex.Message}");
    }
}


        /**
         * @param id The ID of the SDG target to delete.
         * @return IActionResult with 204 if successful or 404 if not found.
         */
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var deletedModel = await _sdgTargetRepo.DeleteAsync(id); 
            if(deletedModel == null){
                return NotFound($"SDG Target with ID {id} not found.");
            }
            return NoContent();
        }
        /**
         * @param id The ID of the SDG target to update.
         * @param updateSdgTargetRequestDto The updated data for the SDG target.
         * @return IActionResult containing the updated SDG target data or 404 if not found.
         */
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSdgTargetRequestDto updateSdgTargetRequestDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var existingSdgTarget = await _sdgTargetRepo.GetByIdAsync(id);
            if (existingSdgTarget == null){
                return NotFound($"SDG Target with ID {id} not found."); 
            }

            existingSdgTarget.Title = updateSdgTargetRequestDto.Title;
            existingSdgTarget.Description = updateSdgTargetRequestDto.Description;
            existingSdgTarget.IsDone = updateSdgTargetRequestDto.IsDone;
            existingSdgTarget.IsPublished = updateSdgTargetRequestDto.IsPublished;

            if (updateSdgTargetRequestDto.SdgTypeIds != null)
            {
                var existingSdgTypeIds = existingSdgTarget.SdgTargetSdgTypes
                    .Select(st => st.SdgType.Id)
                    .OrderBy(id => id) // IDs sortieren für Vergleich
                    .ToList();

                var newSdgTypeIds = updateSdgTargetRequestDto.SdgTypeIds
                    .OrderBy(id => id) // IDs sortieren für Vergleich
                    .ToList();

                // Nur aktualisieren, wenn sich die SDG-Typen geändert haben
                if (!existingSdgTypeIds.SequenceEqual(newSdgTypeIds))
                {
                    // Lade die neuen SDG-Typen
                    var newSdgTypes = await _sdgTypeRepo.GetByIdsAsync(updateSdgTargetRequestDto.SdgTypeIds);
                    if (newSdgTypes.Count != updateSdgTargetRequestDto.SdgTypeIds.Count)
                    {
                        return BadRequest("Some SDG types were not found.");
                    }

                    // Entferne alle alten SDG-Typen-Zuordnungen
                    existingSdgTarget.SdgTargetSdgTypes.Clear();

                    // Füge neue Zuordnungen hinzu
                    foreach (var sdgType in newSdgTypes)
                    {
                        existingSdgTarget.SdgTargetSdgTypes.Add(new SdgTargetSdgType
                        {
                            SdgTarget = existingSdgTarget,
                            SdgType = sdgType
                        });
                    }
                }
            }

            var updatedSdgTarget = await _sdgTargetRepo.UpdateAsync(id, existingSdgTarget);

            if (updatedSdgTarget == null)
            {
                return NotFound($"SDG Target with ID {id} not found after update.");
            }
             return Ok(updatedSdgTarget.ToSdgTargetDto());
        }
    }
}