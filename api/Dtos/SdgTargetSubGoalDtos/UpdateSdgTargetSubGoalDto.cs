using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.SdgTargetSubGoalDtos
{
    public class UpdateSdgTargetSubGoalDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 characters")]
        [MaxLength(280, ErrorMessage =  "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty; 

        [Required]
        [MinLength(5, ErrorMessage = "Description must be 5 characters")]
        [MaxLength(280, ErrorMessage =  "Description cannot be over 280 characters")]
        public string Description { get; set; } = string.Empty; 
    }
}