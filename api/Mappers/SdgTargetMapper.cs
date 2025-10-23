using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.SdgTargetDtos;
using api.Dtos.SdgTypeDtos;
using api.Models;


namespace api.Mappers
{
    public static class SdgTargetMapper
    {
        // this wird verwendet, um eine Erweiterungsmethode zu definieren 
        // diese ermöglichen es, Methoden für bestehende Typen hizuzufügen 
        // ohne diese Typen zu ändern 
        // werden mit Punktoperator aufgerufen 
        // müssen static sein und this vor dem ersten Parameter 
        // dadurch wird der erste Parameter zum Typen auf dem die Methode aufgerufen werden kann 
        public static SdgTargetDto ToSdgTargetDto(this SdgTarget sdgTargetModel ){
            if (sdgTargetModel == null) 
                throw new ArgumentNullException(nameof(sdgTargetModel));

            return new SdgTargetDto
            { 
                Id = sdgTargetModel.Id,
                Title = sdgTargetModel.Title,
                Description = sdgTargetModel.Description,
                IsDone = sdgTargetModel.IsDone,
                IsPublished = sdgTargetModel.IsPublished,
                CompanyName = sdgTargetModel.Company?.Name ?? "Kein Unternehmen", 
                IndustryId = sdgTargetModel.Company?.IndustryId,
                CreatedUserId = sdgTargetModel.CreatedByUserId ?? "Keine User id",
                SubGoals = sdgTargetModel.SubGoals.Select(s => s.ToSdgTargetSubGoalDto()).ToList(),
                SdgTypes = sdgTargetModel.SdgTargetSdgTypes != null
                    ? sdgTargetModel.SdgTargetSdgTypes
                        .Where(st => st.SdgType != null) // Ensure SdgType is not null
                        .Select(st => st.SdgType.ToSdgTypeDto())
                        .ToList() 
                    : new List<SdgTypeDto>() // Default to empty list if null
            };
        }

        public static SdgTarget ToSdgTargetFromCreate(this CreateSdgTargetDto createSdgTargetDto, int companyId){
            return new SdgTarget{

                Title = createSdgTargetDto.Title,
                Description = createSdgTargetDto.Description,
                IsDone = createSdgTargetDto.IsDone,  
                IsPublished = createSdgTargetDto.IsPublished, 
                CompanyId = companyId, 
            };
        }
        public static SdgTarget ToSdgTargetFromUpdate(this UpdateSdgTargetRequestDto updateSdgTargetRequestDto){
            return new SdgTarget{
                Title = updateSdgTargetRequestDto.Title,
                Description = updateSdgTargetRequestDto.Description,
                IsDone = updateSdgTargetRequestDto.IsDone,
                IsPublished = updateSdgTargetRequestDto.IsPublished,
                
            };
        }
    }
}