using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

/**
* @brief Controller for handling account-related operations, such as login and token generation.
* 
* @details The AccountController provides endpoints for user authentication, 
* including login functionality and token management. It relies on ASP.NET Core's
* identity system to manage users and sign-in operations.
*/
namespace api.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly SignInManager<AppUser> _signInManager;

		/**
		* @param userManager Manages user-related operations.
		* @param tokenService Generates tokens for authenticated users.
		* @param signInManager Handles sign-in and authentication processes.
		*/
		public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_signInManager = signInManager;
		}

		/**
		* @brief Handles user login attempts by validating credentials and returning user data with a token.
		* 
		* @param loginDto The login data transfer object containing username and password.
		* @return IActionResult with user data and token if successful, or an error message if not.
		*/
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			// Validates model state (data annotations in LoginDto).
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
			if (existingUser == null)
			{
				return Unauthorized("Invalid Username");
			}

			var isPasswordValid = await _signInManager.CheckPasswordSignInAsync(existingUser, loginDto.Password, false);
			if (!isPasswordValid.Succeeded)
			{
				return Unauthorized("Username not found and/or password incorrect");
			}

			// create token 
			var token = await _tokenService.CreateToken(existingUser);
			// safe token in http-only cookie
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTimeOffset.Now.AddDays(7),
				Secure = false, // auf tru wenn es auf server läuft
				SameSite = SameSiteMode.Lax
			};
			// sends cookie to the client
			Response.Cookies.Append("jwt", token, cookieOptions);

			var roles = await _userManager.GetRolesAsync(existingUser);
			var roleString = roles.FirstOrDefault() ?? "keine Rolle vorhanden";

			var userDto = new AppUserDto
			{
				Id = existingUser.Id,
				Username = existingUser.UserName,
				Email = existingUser.Email,
				CompanyName = existingUser.CompanyId.ToString(),
				Token = token,
				Role = roleString,
				CompanyId = existingUser.CompanyId,
			};

			return Ok(userDto);
		}

		/**
		* @brief Handles user registrations by validating credentials and returning user data with a token.
		* 
		* @param companyId The ID of the company the user is being assigned to.
		* @param registerDto The registration data transfer object containing user credentials.
		* @return IActionResult containing user data and a token if successful, or an error message if not.
		*/
		[HttpPost("register/{companyId:int}")]
		public async Task<IActionResult> Register([FromRoute] int companyId, [FromBody] RegisterDto registerDto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var appUser = new AppUser
				{
					UserName = registerDto.Username,
					Email = registerDto.Email,
					Name = registerDto.Name,
					PhoneNumber = registerDto.PhoneNumber,
					CompanyId = companyId,
				};

				var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
				if (!createdUser.Succeeded)
				{
					return StatusCode(500, createdUser.Errors);
				}

				var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
				if (!roleResult.Succeeded)
				{
					return StatusCode(500, roleResult.Errors);
				}

				var roles = await _userManager.GetRolesAsync(appUser);

				var userDto = new AppUserDto
				{
					Id = appUser.Id,
					Username = appUser.UserName,
					Email = appUser.Email,
					CompanyId = appUser.CompanyId,
					CompanyName = appUser.Company?.Name,
					Role = roles.FirstOrDefault() ?? "keine Rolle vorhanden",
					Token = "noch kein Token gesetzt"
				};

				return Ok(userDto);
			}
			catch (Exception e)
			{
				return StatusCode(500, e);
			}
		}

		/**
* @brief -
* 
* @return -
*/
		[Authorize]
		[HttpGet("me")]
		public IActionResult GetCurrentUser()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userName = User.FindFirstValue(ClaimTypes.Name);
			var email = User.FindFirstValue(ClaimTypes.Email);
			var orgId = User.FindFirst("org_is")?.Value;
			var role = User.FindFirst(ClaimTypes.Role)?.Value;

			if (userId == null)
			{
				return Unauthorized("No user ID found in token.");
			}

			return Ok(new
			{
				Id = userId,
				Username = userName,
				Email = email,
				OrgId = orgId,
				Role = role
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UserUpdateDto updateDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound("User not found");
			}

			user.Email = updateDto.Email;
			user.UserName = updateDto.Username;
			user.PhoneNumber = updateDto.PhoneNumber;
			user.Name = updateDto.Name ?? "User besitzt keinen Name"; // gucken ob das belegt ist

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				return StatusCode(500, result.Errors);
			}

			var userDto = new AppUserDto
			{
				Id = user.Id,
				Username = user.UserName,
				Email = user.Email,
				CompanyId = user.CompanyId,
				CompanyName = user.Company?.Name
			};

			return Ok(userDto);
		}

		[Authorize]
		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Unauthorized();
			}

			var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok("Password changed successfully");
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("all-users")]
		public async Task<IActionResult> GetAllUsers()
		{
			var allUsers = await _userManager.Users
				.Include(u => u.Company)
				.ToListAsync();

			var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

			var nonAdminUsers = allUsers
				.Where(u => !adminUsers.Any(admin => admin.Id == u.Id))
				.ToList();


			var userDtos = nonAdminUsers.Select(u => new AppUserDto
			{
				Id = u.Id,
				Username = u.UserName,
				Email = u.Email,
				Name = u.Name,                      // ➕ NEU
				PhoneNumber = u.PhoneNumber,        // ➕ NEU
				CompanyId = u.CompanyId,
				CompanyName = u.Company?.Name,
			}).ToList();

			return Ok(userDtos);
		}
		// Debug endpunkt 

		[Authorize]
		[HttpGet("debug-user")]
		public async Task<IActionResult> DebugUser()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return Unauthorized("Could not find user ID in token.");
			}

			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

			if (user == null)
			{
				return Unauthorized("Could not find user in Identity store.");
			}

			var roles = await _userManager.GetRolesAsync(user);
			return Ok(new
			{
				user.Id,
				user.UserName,
				roles
			});
		}

		// [Authorize]
		[HttpGet("user-client/{id}")]

		public async Task<ActionResult<UserClientDto>> GetUserById(string id)
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
			string? address = "User has no Company";

			if (user == null)
				return NotFound();

			var roles = await _userManager.GetRolesAsync(user);

			if (roles == null || !roles.Any())
				return BadRequest("User has no role assigned.");

			if (!Enum.TryParse<Role>(roles.First(), ignoreCase: true, out var parsedRole))
				return BadRequest("User role is invalid");


			if (user.Company != null)
			{
				address = $"{user.Company.Street} {user.Company.StreetNumber}, {user.Company.Postalcode} {user.Company.City}";
			}


			var userClientDto = new UserClientDto
			{
				UserId = id,
				Username = user.UserName,
				UserRole = parsedRole,
				Name = user.Name,
				Email = user.Email,
				Address = address,
				PhoneNumber = user.PhoneNumber,
			};

			return Ok(userClientDto);
		}

		[HttpPost("logout")]
		public IActionResult Logout()
		{
				// Cookie mit leerem Wert setzen und Ablaufdatum in der Vergangenheit
				var cookieOptions = new CookieOptions
				{
						HttpOnly = true,
						Secure = false, // Setze auf true, wenn du über HTTPS arbeitest
						SameSite = SameSiteMode.Lax,
						Expires = DateTimeOffset.UtcNow.AddDays(-1) // Sofort ablaufen lassen
				};

				Response.Cookies.Append("jwt", "", cookieOptions);

				return Ok(new { message = "Logout erfolgreich. JWT-Cookie entfernt." });
		}

	}
}