using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.SdgTargetSubGoalDtos;
using api.Models;

namespace api.Dtos.SdgTargetDtos
{
    public class CreateSdgTargetDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Title is too long")]
        public string Title { get; set; } = string.Empty; 
        [Required]
        [MaxLength(400, ErrorMessage = "Description is too long")]
        public string Description { get; set; } = string.Empty; 
        [Required]
        // hier gucken ob Datenüberprüfung notwendig ist
        public bool IsDone { get; set; }
        public bool IsPublished { get; set; }
        [Required]
        public List<int> SdgTypeIds { get; set; } = new List<int>(); // Liste der SdgType IDs
        public List<CreateSdgTargetSubGoalDto> SubGoals {get; set; } = new List<CreateSdgTargetSubGoalDto>(); 
        

    }
}