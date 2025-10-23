using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.SdgTargetSubGoalDtos;
using api.Dtos.SdgTypeDtos;

namespace api.Dtos.SdgTargetDtos
{
    public class SdgTargetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public bool IsPublished { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public int? IndustryId { get; set; } 
        public string CreatedUserId { get; set; } = string.Empty; 
        public List<SdgTypeDto> SdgTypes { get; set; } = new List<SdgTypeDto>();
        public List<SdgTargetSubGoalDto> SubGoals {get; set; } = new List<SdgTargetSubGoalDto>(); 
        
    }
}