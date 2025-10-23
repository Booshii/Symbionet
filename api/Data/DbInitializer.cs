using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Helpers;

namespace api.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeRolesAndUsersAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
                var context = scopedServices.GetRequiredService<ApplicationDBContext>();

                // Define roles
                var roles = new[] { "Admin", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Data from appsettings.json
                string adminUserName = configuration["AdminUser:UserName"];
                string adminEmail = configuration["AdminUser:Email"];
                string adminPassword = configuration["AdminUser:Password"];
                string adminRole = configuration["AdminUser:Role"];
                string adminName = configuration["AdminUser:Name"];

                if (string.IsNullOrWhiteSpace(adminUserName) ||
                    string.IsNullOrWhiteSpace(adminEmail) ||
                    string.IsNullOrWhiteSpace(adminPassword) ||
                    string.IsNullOrWhiteSpace(adminRole))
                {
                    throw new Exception("Admin user settings are missing or incomplete in appsettings.json.");
                }

                // Admin User erstellen
                if (await userManager.FindByNameAsync(adminUserName) == null)
                {
                    var adminUser = new AppUser
                    {
                        UserName = adminUserName,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Name = adminName,

                    };
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }

                 // Industries seeden
                if (!context.Industries.Any())
                {
                    var industries = new[] {
                        new Industry {
                            Id = 1,
                            Name = "Metall"
                        },
                        new Industry {
                            Id = 2,
                            Name = "Holz"
                        },
                        new Industry {
                            Id = 3,
                            Name = "Plastik"
                        }

                    };
                    context.Industries.AddRange(industries);
                    await context.SaveChangesAsync();

                }
                // Companies seeden
                if (!context.Companies.Any())
                {
                    var companies = new[] {
                        new Company {
                            Name = "Grüne Zukunft GmbH",
                            Street = "Motzener Straße",
                            StreetNumber = "26",
                            Postalcode = 12277,
                            City = "Berlin",
                            Latitude = "",
                            Longitude = "",
                            ContactPerson = "Anna Bauer",
                            ContactEmail = "anna.bauer@gruenezukunft.de",
                            ContactTel = "03012345678",
                            IndustryId = 1, // Stelle sicher, dass eine Industry mit dieser ID existiert!
                            WebsiteLink = "https://www.gruenezukunft.de"
                        },
                        new Company {
                            Name = "TechNova AG",
                            Street = "Sperenberger Straße",
                            StreetNumber = "6",
                            Postalcode = 12277,
                            City = "Berlin",
                            Latitude = "",
                            Longitude = "",
                            ContactPerson = "Maximilian Richter",
                            ContactEmail = "m.richter@technova.de",
                            ContactTel = "08998765432",
                            IndustryId = 2,
                            WebsiteLink = "https://www.technova.de"
                        },
                        new Company {
                            Name = "FairFood e.V.",
                            Street = "Richard-Tauber-Damm",
                            StreetNumber = "11",
                            Postalcode = 12277,
                            City = "Berlin",
                            Latitude = "",
                            Longitude = "",
                            ContactPerson = "Laura Schmidt",
                            ContactEmail = "laura.schmidt@fairfood.org",
                            ContactTel = "07111234567",
                            IndustryId = 3,
                            WebsiteLink = "https://www.fairfood.org"
                        }
                    };

                    // Geocoder für Koordinaten benutzen 
                    foreach (var company in companies)
                    {
                        var address = $"{company.Street} {company.StreetNumber}, {company.Postalcode} {company.City}, Deutschland";
                        var coordinates = await GeocodingHelper.GetCoordinatesFromAddress(address);
                        if (coordinates != null) {
                            company.Latitude = coordinates.Value.Latitude.ToString();
                            company.Longitude = coordinates.Value.Longitude.ToString();
                        } else {
                            Console.WriteLine($"[Seeder] ⚠️ Keine Koordinaten für: {address}");
                            company.Latitude = "0";
                            company.Longitude = "0";
                        }
                    }


                    context.Companies.AddRange(companies);
                    await context.SaveChangesAsync();
                }

                // Zwei zusätzliche User erstellen
                var additionalUsers = new[]
                {
                    new { Id = "1", UserName = "user1", Email = "user1@example.com", Password = "User1Password123!", Role = "User", Name = "Selmer Müller", CompanyId = 1 },
                    new { Id = "2", UserName = "user2", Email = "user2@example.com", Password = "User2Password123!", Role = "User", Name = "Marita Knop", CompanyId = 2 }
                };

                foreach (var userInfo in additionalUsers)
                {
                    if (await userManager.FindByNameAsync(userInfo.UserName) == null)
                    {
                        var user = new AppUser
                        {
                            Id = userInfo.Id,
                            UserName = userInfo.UserName,
                            Email = userInfo.Email,
                            EmailConfirmed = true,
                            Name = userInfo.Name,
                            CompanyId = userInfo.CompanyId,
                        };
                        var createResult = await userManager.CreateAsync(user, userInfo.Password);
                        if (!createResult.Succeeded)
                        {
                            throw new Exception($"Failed to create user {userInfo.UserName}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                        }


                        if (await roleManager.RoleExistsAsync(userInfo.Role))
                        {
                            await userManager.AddToRoleAsync(user, userInfo.Role);
                        }
                        else
                        {
                            throw new Exception($"Role '{userInfo.Role}' does not exist.");
                        }
                    }
                }

                // SDG Types seeden (inklusive optional SDG 17)
                if (!context.SdgTypes.Any())
                {
                    var sdgTypes = new[]
                    {
                        new SdgType { Name = "No Poverty", Color = "hsl(0, 81%, 52%)", Number="01" },
                        new SdgType { Name = "Zero Hunger", Color = "hsl(41, 69%, 55%)", Number="02" },
                        new SdgType { Name = "Good Health and Well-being", Color = "hsl(134, 47%, 42%)", Number="03" },
                        new SdgType { Name = "Quality Education", Color = "hsl(0, 81%, 39%)", Number="04" },
                        new SdgType { Name = "Gender Equality", Color = "hsl(9, 100%, 56%)", Number="05" },
                        new SdgType { Name = "Clean Water and Sanitation", Color = "hsl(192, 79%, 52%)", Number="06" },
                        new SdgType { Name = "Affordable and Clean Energy", Color = "hsl(48, 96%, 58%)", Number="07" },
                        new SdgType { Name = "Decent Work and Economic Growth", Color = "hsl(339, 68%, 39%)", Number="08" },
                        new SdgType { Name = "Industry, Innovation and Infrastructure", Color = "hsl(18, 100%, 59%)", Number="09" },
                        new SdgType { Name = "Reduced Inequality", Color = "hsl(327, 81%, 42%)", Number="10" },
                        new SdgType { Name = "Sustainable Cities and Communities", Color = "hsl(29, 98%, 56%)", Number="11" },
                        new SdgType { Name = "Responsible Consumption and Production", Color = "hsl(35, 54%, 47%)", Number="12" },
                        new SdgType { Name = "Climate Action", Color = "hsl(123, 35%, 39%)", Number="13" },
                        new SdgType { Name = "Life Below Water", Color = "hsl(202, 91%, 43%)", Number="14" },
                        new SdgType { Name = "Life on Land", Color = "hsl(96, 65%, 46%)", Number="15" },
                        new SdgType { Name = "Peace and Justice Strong Institutions", Color = "hsl(203, 100%, 31%)", Number="16" },
                        new SdgType { Name = "Partnerships for the Goals", Color = "hsl(202, 71%, 53%)", Number="17" }
                    };

                    context.SdgTypes.AddRange(sdgTypes);
                    await context.SaveChangesAsync();
                }
               
                // sdgTargets seeden
                if (!context.SdgTargets.Any())
                {
                    var now = DateTime.Now; 

                    var sdgTargets = new[] {
                        // Grüne Zukunft GmbH (CompanyId = 1)
                        new SdgTarget {
                        Title = "Photovoltaik-Anlage auf dem Firmendach",
                        Description = "Installation einer Solaranlage zur Eigenstromversorgung aus erneuerbaren Energien.",
                        IsDone = true,
                        IsPublished = true,
                        CreationDate = now,
                        CompanyId = 1,
                        CreatedByUserId = "2", 
                        SdgTargetSdgTypes = new List<SdgTargetSdgType>
                            {
                                new SdgTargetSdgType { SdgTypeId = 7 },
                                new SdgTargetSdgType { SdgTypeId = 12 },
                                new SdgTargetSdgType { SdgTypeId = 13 }
                            }
                        },
                        new SdgTarget {
                            Title = "Jobrad für Mitarbeiter",
                            Description = "Einführung eines Dienstradleasings zur Förderung nachhaltiger Mobilität.",
                            IsDone = false,
                            IsPublished = true,
                            CreationDate = now,
                            CompanyId = 1,
                            CreatedByUserId = "2",
                            SdgTargetSdgTypes = new List<SdgTargetSdgType>
                            {
                                new SdgTargetSdgType { SdgTypeId = 3 },
                                new SdgTargetSdgType { SdgTypeId = 11 },
                                new SdgTargetSdgType { SdgTypeId = 13 }
                            }
                        },

                        // TechNova AG (CompanyId = 2)
                        new SdgTarget {
                            Title = "Serverraum mit Abwärmenutzung",
                            Description = "Nutzung der Serverabwärme zur Heizungsunterstützung im Bürogebäude.",
                            IsDone = true,
                            IsPublished = true,
                            CreationDate = now,
                            CompanyId = 2,
                            CreatedByUserId = "1", 
                            SdgTargetSdgTypes = new List<SdgTargetSdgType>
                            {
                                new SdgTargetSdgType { SdgTypeId = 7 },
                                new SdgTargetSdgType { SdgTypeId = 9 },
                                new SdgTargetSdgType { SdgTypeId = 13 }
                            }
                        },
                        new SdgTarget {
                            Title = "Recycling von Elektronikkomponenten",
                            Description = "Rücknahme und Aufbereitung ausgedienter Elektronikbauteile.",
                            IsDone = false,
                            IsPublished = true,
                            CreationDate = now,
                            CompanyId = 2,
                            CreatedByUserId = "1", 
                            SdgTargetSdgTypes = new List<SdgTargetSdgType>
                            {
                                new SdgTargetSdgType { SdgTypeId = 9 },
                                new SdgTargetSdgType { SdgTypeId = 12 }
                            }
                        },

                        // FairFood e.V. (CompanyId = 3)
                        new SdgTarget  {
                            Title = "Kooperation mit regionalen Biohöfen",
                            Description = "Bezug aller Lebensmittel von zertifizierten Biohöfen aus der Region.",
                            IsDone = true,
                            IsPublished = true,
                            CreationDate = now,
                            CompanyId = 3,
                            CreatedByUserId = "2",
                            SdgTargetSdgTypes = new List<SdgTargetSdgType>
                            {
                                new SdgTargetSdgType { SdgTypeId = 2 },
                                new SdgTargetSdgType { SdgTypeId = 12 },
                                new SdgTargetSdgType { SdgTypeId = 15 }
                            }
                        },
                        new SdgTarget {
                            Title = "Bildungsprojekt zu nachhaltiger Ernährung",
                            Description = "Workshops an Schulen zur Förderung von Wissen über nachhaltige Ernährung.",
                            IsDone = false,
                            IsPublished = true,
                            CreationDate = now,
                            CompanyId = 3,
                            CreatedByUserId = "2",
                            SdgTargetSdgTypes = new List<SdgTargetSdgType>
                            {
                                new SdgTargetSdgType { SdgTypeId = 4 },
                                new SdgTargetSdgType { SdgTypeId = 12 },
                                new SdgTargetSdgType { SdgTypeId = 13 }
                            }
                        }
                    };

                    context.SdgTargets.AddRange(sdgTargets);
                    await context.SaveChangesAsync();
                    
                }
            }
        }
    }
}
