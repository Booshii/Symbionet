using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Company;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

/**
* @brief Controller for handling company-related operations
*
* @details The CompanyController provides endpoints for CRUS operations

// vielleicht das eher zum schluss schreiben 
Endpoints 
create

*/
namespace api.Controllers
{
	[Route("api/company")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		private readonly ICompanyRepository _companyRepo; 
		private readonly IIndustryRepository _industryRepo; 
		private readonly UserManager<AppUser> _userManager;
		/**
			* @param companyRepo The repository handling company-related database operations.
			* @param industryRepo The repository for verifying and retrieving industry information.
			*/
		public CompanyController(ICompanyRepository companyRepo, IIndustryRepository industryRepo, UserManager<AppUser> userManager){
			_companyRepo = companyRepo;
			_industryRepo = industryRepo;
			_userManager = userManager;
		}
		/**
			* @brief Retrieves all companies.
			* 
			* @return IActionResult containing a list of companies with their plain targets.
			*/
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var companiesFromDB = await _companyRepo.GetAllAsync();
			var companyDto = companiesFromDB.Select(c => c.ToCompanyDtoWithPlainTargets());
			return Ok(companyDto);
		}

		/**
			* @brief Retrieves companies for the home view.
			* 
			* @return IActionResult containing a list of companies in the home view format.
			*/
		[HttpGet("homeview")]
		public async Task<IActionResult> GetAllForHomeview()
		{
			var companiesFromDB = await _companyRepo.GetAllAsync();
			var homeViewCompaniesDto = companiesFromDB.Select(c => c.ToHomeViewCompanyDto());
			return Ok(homeViewCompaniesDto);
		}

		/**
			* @brief Retrieves a specific company by ID.
			* 
			* @param id The ID of the company to retrieve.
			* @return IActionResult containing the company data or a 404 status if not found.
			*/
		
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{

			// var user = await _userManager.GetUserAsync(User); 
			// if (user == null)
			// 	return NotFound();

			// var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

			// if (!isAdmin && user.CompanyId != id){
			// 	return Forbid(); 
			// }
			
			var companyFromDB = await _companyRepo.GetByIdAsync(id);
			if (companyFromDB == null)
					return NotFound();

			var companyDto = companyFromDB.ToCompanyDtoWithPlainTargets();
			return Ok(companyDto);
		}

		/**
			* @brief Retrieves a specific company with grouped targets by ID.
			* 
			* @param id The ID of the company to retrieve.
			* @return IActionResult containing the company data with grouped targets or a 404 status if not found.
			*/
		[HttpGet("groupedTargets/{id:int}")]
		public async Task<IActionResult> GetByIdWithGroupedTargets(int id)
		{
			var companyFromDB = await _companyRepo.GetByIdAsync(id);
			if (companyFromDB == null)
					return NotFound();

			var groupedTargetsCompanyDto = companyFromDB.ToGroupedTargetsCompanyDto();
			return Ok(groupedTargetsCompanyDto);
		}

		/**
			* @brief Creates a new company and associates it with an industry.
			* 
			* @param industryId The ID of the industry to associate with the company.
			* @param createCompanyDto The data transfer object containing company details.
			* @return IActionResult with the created company data or an error message if the industry does not exist.
			* 
			* @details If latitude and longitude are missing in the provided DTO, the method uses the GeocodingHelper to fetch
			* geographic coordinates based on the provided address. Validates the industry association and adds the company
			* to the repository.
			*/
		[HttpPost("{industryId:int}")]
		public async Task<IActionResult> Create([FromRoute] int industryId, [FromBody] CreateCompanyDto createCompanyDto)
		{
			if (!ModelState.IsValid)
					return BadRequest(ModelState);

			var industryExists = await _industryRepo.IndustryExists(industryId);
			if (!industryExists)
					return BadRequest("Industry does not exist");

			// Wenn Lat und Lon fehlen, Koordinaten über Nominatim API ermitteln
			if (string.IsNullOrEmpty(createCompanyDto.Latitude) || string.IsNullOrEmpty(createCompanyDto.Longitude)) {
				var address = $"{createCompanyDto.Street} {createCompanyDto.StreetNumber}, {createCompanyDto.Postalcode} {createCompanyDto.City}, Deutschland";
				Console.WriteLine($"Adresse für Nominatim: {address}");
				var coordinates = await GeocodingHelper.GetCoordinatesFromAddress(address);

				if (coordinates == null)
				{
						return BadRequest("Unable to determine coordinates for the given address.");
				}

				createCompanyDto.Latitude = coordinates.Value.Latitude.ToString();
				createCompanyDto.Longitude = coordinates.Value.Longitude.ToString();
			}
			var companyModel = createCompanyDto.ToCompanyFromCreateDTO(industryId);
			await _companyRepo.CreateAsync(companyModel);

			var companyDto = companyModel.ToCompanyDtoWithPlainTargets();
			return CreatedAtAction(nameof(GetById), new { id = companyModel.Id }, companyDto);
		}

		/**
			* @brief Creates a new company based on file upload data.
			* 
			* @param createCompanyDto The data transfer object containing company details from a file.
			* @return IActionResult with the created company data or an error message if the industry does not exist.
			*/
		[HttpPost("fileUpload")]
		public async Task<IActionResult> UploadJson([FromBody] List<CreateCompanyFileUploadDto> jsonDataCompanies)
		{
			try{
				if (jsonDataCompanies == null || jsonDataCompanies.Count == 0){
					return BadRequest("Keine Datei hochgeladen oder leere Daten.");
				}

				List<CompanyDto> savedCompanies = new List<CompanyDto>();

				foreach (var createCompanyDto in jsonDataCompanies){
					if (!ModelState.IsValid){
						return BadRequest(ModelState);
					}

					var industryExists = await _industryRepo.IndustryExists(createCompanyDto.IndustryId);
					if (!industryExists){
						return BadRequest($"Industry does not exist for Company: {createCompanyDto.Name}");
					}

					// Falls Latitude oder Longitude fehlen, Koordinaten über die Adresse bestimmen
					if (string.IsNullOrEmpty(createCompanyDto.Latitude) || string.IsNullOrEmpty(createCompanyDto.Longitude))
					{
						var address = $"{createCompanyDto.Street} {createCompanyDto.StreetNumber}, {createCompanyDto.Postalcode} {createCompanyDto.City}, Deutschland";
						Console.WriteLine($"Adresse für Geocoding: {address}");
						var coordinates = await GeocodingHelper.GetCoordinatesFromAddress(address);

						if (coordinates == null)
						{
								return BadRequest($"Unable to determine coordinates for the given address: {createCompanyDto.Name}");
						}

						createCompanyDto.Latitude = coordinates.Value.Latitude;
						createCompanyDto.Longitude = coordinates.Value.Longitude;
					}

					var companyModel = createCompanyDto.ToCompanyFromFileUpload();
					await _companyRepo.CreateAsync(companyModel);

					var companyDto = companyModel.ToCompanyDtoWithPlainTargets();
					savedCompanies.Add(companyDto);
				}

				return Ok(new { message = "Daten erfolgreich empfangen", savedData = savedCompanies });
			} catch (Exception ex) {
				return StatusCode(500, $"Fehler beim Verarbeiten der Datei: {ex.Message}");
			}
		}
			/**
				* @brief Updates an existing company.
				* 
				* @param id The ID of the company to update.
				* @param updateCompanyRequestDto The data transfer object containing updated company details.
				* @return IActionResult with the updated company data or a 404 status if not found.
				*/
			[HttpPut("{id:int}")]
			public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCompanyRequestDto updateCompanyRequestDto)
			{
					if (!ModelState.IsValid)
							return BadRequest(ModelState);

					var companyModel = await _companyRepo.UpdateAsync(id, updateCompanyRequestDto);
					if (companyModel == null)
							return NotFound();

					var companyDto = companyModel.ToCompanyDtoWithPlainTargets();
					return Ok(companyDto);
			}

		/**
			* @brief Deletes a company by ID.
			* 
			* @param id The ID of the company to delete.
			* @return IActionResult with no content if successful or a 404 status if not found.
			*/
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
				if (!ModelState.IsValid)
						return BadRequest(ModelState);

				var companyModel = await _companyRepo.DeleteAsync(id);
				if (companyModel == null)
						return NotFound();

				return NoContent();
		}
	}
}
