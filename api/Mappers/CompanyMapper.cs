using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Company;
using api.Dtos.SdgTargetDtos;
using api.Dtos.SdgTypeDtos;
using api.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace api.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyDto ToCompanyDtoWithPlainTargets(this Company companyModel){
            return new CompanyDto{
                Id = companyModel.Id,
                Name = companyModel.Name,
                
                Street = companyModel.Street,
                StreetNumber = companyModel.StreetNumber,
                Postalcode = companyModel.Postalcode,
                City = companyModel.City,
                ContactPerson = companyModel.ContactPerson,
                ContactEmail = companyModel.ContactEmail,
                ContactTel = companyModel.ContactTel,
                IndustryName = companyModel.Industry?.Name ?? "Keine Branche ",
                Latitude = companyModel.Latitude,
                Longitude = companyModel.Longitude,
                WebsiteLink = companyModel.WebsiteLink,
                
                // ************ nochmal gucken ob die wirklich gebraucht werden 
                SdgTargets = companyModel.SdgTargets.Select(s => s.ToSdgTargetDto()).ToList(),

                // hier kann man das                 


                AppUsers = companyModel.AppUsers
            };
        }

        public static groupedTargetsCompanyDto ToGroupedTargetsCompanyDto(this Company companyModel){
            
            var groupedTargets = new Dictionary<string, List<SdgTargetDto>>(); 
            foreach (var target in companyModel.SdgTargets)
            {
                var targetDto = target.ToSdgTargetDto(); 
                
                foreach (var type in targetDto.SdgTypes)
                {
                    var sdgTypeId = type.Id.ToString();

                    if(!groupedTargets.ContainsKey(sdgTypeId)) {
                        groupedTargets[sdgTypeId] = new List<SdgTargetDto>();
                    }
                    groupedTargets[sdgTypeId].Add(targetDto); 
                }
            }
            
            
            return new groupedTargetsCompanyDto{
                Id = companyModel.Id,
                Name = companyModel.Name,
                
                Street = companyModel.Street,
                StreetNumber = companyModel.StreetNumber,
                Postalcode = companyModel.Postalcode,
                City = companyModel.City,
                ContactPerson = companyModel.ContactPerson,
                ContactEmail = companyModel.ContactEmail,
                ContactTel = companyModel.ContactTel,
                IndustryName = companyModel.Industry?.Name ?? "Keine Branche ",
                Latitude = companyModel.Latitude,
                Longitude = companyModel.Longitude,
                WebsiteLink = companyModel.WebsiteLink,
                
                // ************ nochmal gucken ob die wirklich gebraucht werden 
                GroupedSdgTargetsByColor = groupedTargets,
                AppUsers = companyModel.AppUsers
            };
        }
        public static Company ToCompanyFromCreateDTO(this CreateCompanyDto companyDto, int industryId){
            return  new Company{
                
                Name = companyDto.Name,
                
                Street = companyDto.Street,
                StreetNumber = companyDto.StreetNumber,
                Postalcode = companyDto.Postalcode,
                City = companyDto.City,
                ContactPerson = companyDto.ContactPerson,
                ContactEmail = companyDto.ContactEmail,
                ContactTel = companyDto.ContactTel,
                Latitude = companyDto.Latitude,
                Longitude = companyDto.Longitude,
                IndustryId = industryId,
                WebsiteLink = companyDto.WebsiteLink
            };
        }
        public static Company ToCompanyFromFileUpload(this CreateCompanyFileUploadDto companyDto){
            return new Company{
                Name = companyDto.Name,
                Street = companyDto.Street,
                StreetNumber = companyDto.StreetNumber,
                Postalcode = companyDto.Postalcode,
                City = companyDto.City,
                ContactPerson = companyDto.ContactPerson,
                ContactEmail = companyDto.ContactEmail,
                ContactTel = companyDto.ContactTel,
                Latitude = companyDto.Latitude,
                Longitude = companyDto.Longitude,
                IndustryId = companyDto.IndustryId,
                WebsiteLink = companyDto.WebsiteLink 
            };
        }
        public static HomeViewCompanyDto ToHomeViewCompanyDto(this Company companyModel){
            return new HomeViewCompanyDto{

                Id = companyModel.Id,
                Name = companyModel.Name,
                
                Street = companyModel.Street,
                StreetNumber = companyModel.StreetNumber,
                Postalcode = companyModel.Postalcode,
                City = companyModel.City,
                IndustryName = companyModel.Industry?.Name ?? "Keine Branche ",
                IndustryId = companyModel.IndustryId,
                Latitude = companyModel.Latitude,
                Longitude = companyModel.Longitude,
                SdgTypes = companyModel.SdgTargets
                // SelectMany: This is used to iterate over all SdgTargets 
                // and collect their associated SDG types (SdgTargetSdgTypes).
                    .SelectMany(target => target.SdgTargetSdgTypes) 
                    .Select(SdgTargetSdgTypes => new SimpleSdgTypeDto
                    {
                        Id = SdgTargetSdgTypes.SdgType.Id,
                        Name = SdgTargetSdgTypes.SdgType?.Name ?? "Unbekannt"
                    })
                    .ToList()
                
            };
        }
    }
}