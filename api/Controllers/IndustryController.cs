using api.Dtos.Industry;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     /**
     * @brief Controller for managing industries.
     * 
     * @details This controller provides CRUD endpoints for industry operations.
     */
    [Route("api/industry")]
    [ApiController]
    public class IndustryController : ControllerBase
    {
        private readonly IIndustryRepository _industryRepo; 
        /**
         * @param industryRepo The repository for handling industry-related operations.
         */
        public IndustryController(IIndustryRepository industryRepo){
            _industryRepo = industryRepo; 
        }

        /**
         * @brief Retrieves all industries.
         * 
         * @return IActionResult containing a list of industries as DTOs.
         * @details Returns a list of all industries.
         */
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            if(!ModelState.IsValid){
                return BadRequest(ModelState); 
            }
            
            var industries = await _industryRepo.GetAllAsync(); 
            var industryDto = industries.Select(i => i.ToIndustryDto());

            return Ok(industryDto);
        }

        /**
         * @brief Retrieves an industry by its ID.
         * 
         * @param id The ID of the industry to retrieve.
         * @return IActionResult containing the industry's data as a DTO or a 404 status if not found.
         */
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState); 
            }

            var industryModel = await _industryRepo.GetByIdAsync(id);
            if(industryModel == null){
                return NotFound(); 
            }   

            var industryDto = industryModel.ToIndustryDto(); 

            return Ok(industryDto); 
        }
        /**
         * @brief Creates a new industry in Database.
         * 
         * @param createIndustryDto The data of the industry to create.
         * @return IActionResult containing the data of the created industry as a DTO.
         */
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIndustryDto createIndustryDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var industryModel = createIndustryDto.ToIndustryFromCreateDto();

            await _industryRepo.CreateAsync(industryModel);

            var industryDto = industryModel.ToIndustryDto();

            return CreatedAtAction(nameof(GetById), new {id=industryModel.Id}, industryDto); 
        }


        /**
         * @brief Updates an existing industry.
         * 
         * @param id The ID of the industry to update.
         * @param updateIndustryRequestDto The updated data for the industry.
         * @return IActionResult containing the updated industry's data or a 404 status if not found.
         */
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateIndustryRequestDto updateIndustryRequestDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var industryModel = await _industryRepo.UpdateAsync(id, updateIndustryRequestDto); 
            if (industryModel == null){
                return NotFound(); 
            }

            var industryDto = industryModel.ToIndustryDto();

            return Ok(industryDto); 
        }

        /**
         * @brief Deletes an industry by its ID.
         * 
         * @param id The ID of the industry to delete.
         * @return IActionResult with a 204 status if successful or a 404 status if not found.
         */
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var industryModel = await _industryRepo.DeleteAsync(id);
            if(industryModel == null){
                return NotFound();
            }
            return NoContent();
        }
    }
}