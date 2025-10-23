using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.SdgTargetSubGoalDtos;
using api.Models;

namespace api.Mappers
{
    public static class SdgTargetSubGoalMapper
    {
        public static SdgTargetSubGoalDto ToSdgTargetSubGoalDto(this SdgTargetSubGoal sdgTargetSubGoalModel){
            return new SdgTargetSubGoalDto{
                Id = sdgTargetSubGoalModel.Id,
                Title = sdgTargetSubGoalModel.Title,
                Description = sdgTargetSubGoalModel.Description,
                SdgTargetId = sdgTargetSubGoalModel.SdgTargetId,
            };
        }
        
        public static  SdgTargetSubGoal ToSdgTargetSubGoalFromCreate(this CreateSdgTargetSubGoalDto sdgTargetSubGoalDto, int sdgTargetId){
            return new SdgTargetSubGoal{
                // id is created in the controller
                Title = sdgTargetSubGoalDto.Title,
                Description = sdgTargetSubGoalDto.Description,
                SdgTargetId = sdgTargetId

            };
        }

        public static SdgTargetSubGoal ToSdgTargetSubGoalFromUpdate(this UpdateSdgTargetSubGoalDto UpdateSdgTargetSubGoalDto){
            return new SdgTargetSubGoal{
                Title = UpdateSdgTargetSubGoalDto.Title,
                Description = UpdateSdgTargetSubGoalDto.Description,
              
            };
            
        }
    }
}