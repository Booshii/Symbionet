
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using api.Dtos; 


/**
 * @brief Controller for handling statistic-related operations
 * 
 * @details The StatisticController provides endpoints for statistics
 */
namespace api.Controllers
{
  [Route("api/statistic")]
  [ApiController]
  public class StatisticController : ControllerBase
  {
    private readonly ApplicationDBContext _context; 

      /**
      * @param userManager Manages user-related operations.
      * @param tokenService Generates tokens for authenticated users.
      * @param signInManager Handles sign-in and authentication processes.
      */
      public StatisticController(ApplicationDBContext context)
      {
          _context = context;
      }

      [HttpGet("home")]
      public async Task<ActionResult<HomeStatisticsDto>> GetHomeStatistics(){
        var dto = new HomeStatisticsDto {
          Goals = 17,
          Companies = await _context.Companies.CountAsync(),
          Industries = await _context.Industries.CountAsync(), 
          SdgTargets = await _context.SdgTargets.CountAsync(),
        };

        return Ok(dto); 

      }

  }
}