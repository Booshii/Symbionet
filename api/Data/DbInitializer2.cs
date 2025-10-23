// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using System;
// using System.Threading.Tasks;
// using api.Models;

// namespace api.Data
// {
//     public static class DbInitializer
//     {
//         public static async Task InitializeRolesAndUsersAsync(IServiceProvider serviceProvider, IConfiguration configuration)
//         {
//             var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
//             var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//             var context = serviceProvider.GetRequiredService<ApplicationDBContext>();

//             //Define roles
//             var roles = new[] { "Admin", "User" }; 
//             foreach (var role in roles){
//                 var roleExist = await roleManager.RoleExistsAsync(role);
//                 if(!roleExist){
//                     await roleManager.CreateAsync(new IdentityRole(role)); 
//                 }
//             }
//             // Data from appsettings.json
//             string adminUserName = configuration["AdminUser:UserName"];
//             string adminEmail = configuration["AdminUser:Email"];
//             string adminPassword = configuration["AdminUser:Password"];
//             string adminRole = configuration["AdminUser:Role"];

//             // Prüfen und erstellen des Admin-Users
//             if (await userManager.FindByNameAsync(adminUserName) == null)
//             {
//                 var adminUser = new AppUser { UserName = adminUserName, Email = adminEmail, EmailConfirmed = true };
//                 var result = await userManager.CreateAsync(adminUser, adminPassword);
//                 if (result.Succeeded)
//                 {
//                     await userManager.AddToRoleAsync(adminUser, adminRole);
//                 }
//             }

//             // Check and initialize SDG Types 
//             if (!context.SdgTypes.Any())
//                 {
//                     var sdgTypes = new[]
//                     {
//                         new SdgType { Name = "No Poverty", Color = "hsl(0, 81%, 52%)", Number="01" },
//                         new SdgType { Name = "Zero Hunger", Color = "hsl(41, 69%, 55%)", Number="02" },
//                         new SdgType { Name = "Good Health and Well-being", Color = "hsl(134, 47%, 42%)", Number="03" },
//                         new SdgType { Name = "Quality Education", Color = "hsl(0, 81%, 39%)", Number="04" },
//                         new SdgType { Name = "Gender Equality", Color = "hsl(9, 100%, 56%)", Number="05" },
//                         new SdgType { Name = "Clean Water and Sanitation", Color = "hsl(192, 79%, 52%)", Number="06" },
//                         new SdgType { Name = "Affordable and Clean Energy", Color = "hsl(48, 96%, 58%)", Number="07" },
//                         new SdgType { Name = "Decent Work and Economic Growth", Color = "hsl(339, 68%, 39%)", Number="08" },
//                         new SdgType { Name = "Industry, Innovation and Infrastructure", Color = "hsl(18, 100%, 59%)", Number="09" },
//                         new SdgType { Name = "Reduced Inequality", Color = "hsl(327, 81%, 42%)", Number="10" },
//                         new SdgType { Name = "Sustainable Cities and Communities", Color = "hsl(29, 98%, 56%)", Number="11" },
//                         new SdgType { Name = "Responsible Consumption and Production", Color = "hsl(35, 54%, 47%)", Number="12" },
//                         new SdgType { Name = "Climate Action", Color = "hsl(123, 35%, 39%)", Number="13" },
//                         new SdgType { Name = "Life Below Water", Color = "hsl(202, 91%, 43%)", Number="14" },
//                         new SdgType { Name = "Life on Land", Color = "hsl(96, 65%, 46%)", Number="15" },
//                         new SdgType { Name = "Peace and Justice Strong Institutions", Color = "hsl(203, 100%, 31%)", Number="16" },
//                     };

//                     context.SdgTypes.AddRange(sdgTypes);
//                     await context.SaveChangesAsync();
//                 }

//         }
//     }
// }