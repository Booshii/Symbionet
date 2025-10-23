using api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using api.Interfaces;
using api.Repository;
using api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Service;
using System.Text;
using System.Security.Claims;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Load appsettings.{ENV}.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title="My API", Version = "v1"});

	options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
		Name= "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Gib deinen JWT Token hier ein "
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}
);


builder.Services.AddControllers().AddNewtonsoftJson(options => {
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}); 



// Create Database Connection
builder.Services.AddDbContext<ApplicationDBContext>(options => 
{

	var serverVersion = new MySqlServerVersion(new Version(10,5,23));

	// gets the ConnectionsString from appsettings.json 
	options.UseMySql(
			builder.Configuration.GetConnectionString("DefaultConnection"), 
			serverVersion,
			mySqlOptions => mySqlOptions.EnableRetryOnFailure(
					maxRetryCount: 5,
					maxRetryDelay: TimeSpan.FromSeconds(10),
					errorNumbersToAdd: null
			)
	);
});

// Identity Settings  
// Adds identity services to the DI container. 'AppUser' and 'IdentityRole' specify the types for the user and the role.
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
	// Configures the requirements for user passwords.
	options.Password.RequireDigit = true; // Passwords must contain a digit.
	options.Password.RequireLowercase = true; // Passwords must contain a lowercase letter.
	options.Password.RequireUppercase = true; // Passwords must contain an uppercase letter.
	options.Password.RequireNonAlphanumeric = true; // Passwords must contain a non-alphanumeric character.
	options.Password.RequiredLength = 12; // Passwords must be at least 12 characters long.
})
// Links Identity to Entity Framework, using 'ApplicationDBContext' as the database context.
.AddEntityFrameworkStores<ApplicationDBContext>();

// Adds authentication services and configures default schemes for various authentication purposes.
builder.Services.AddAuthentication(options => {
	// Configures all default schemes to 'JwtBearerDefaults.AuthenticationScheme', i.e., JWT Bearer Token.
	options.DefaultAuthenticateScheme = 
	options.DefaultChallengeScheme = 
	options.DefaultForbidScheme = 
	options.DefaultScheme = 
	options.DefaultSignInScheme = 
	options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme; // Sets the default scheme to JWT.
})
// Adds support for JWT Bearer Tokens.
	.AddJwtBearer(options => {
			// Token aus einem Cookie namens "jwt" auslesen
			options.Events = new JwtBearerEvents {
					OnMessageReceived = context => {
							if (context.Request.Cookies.ContainsKey("jwt")) // 🛠 Prüft, ob Cookie existiert
					{
							context.Token = context.Request.Cookies["jwt"]; // 🛠 Token aus Cookie setzen
					}
					return Task.CompletedTask;
					}
			
					// OnMessageReceived = context => {
					//     var token = context.Request.Cookies["jwt"];
					//     if (!string.IsNullOrEmpty(token))
					//     {
					//         context.Token = token; 
					//     }
					//     return Task.CompletedTask; 
					// }
			};
			var signingKey = builder.Configuration["JWT:SigningKey"]
					?? throw new InvalidOperationException("JWT:SigningKey is not configured.");

			options.TokenValidationParameters = new TokenValidationParameters{
					ValidateIssuer = true, // Validates the issuer of the token.
					ValidIssuer = builder.Configuration["JWT:Issuer"], // Sets the valid issuer of the token from the configuration.
					ValidateAudience = true, // Validates the audience of the token.
					ValidAudience = builder.Configuration["JWT:Audience"], // Sets the valid audience of the token from the configuration.
					ValidateIssuerSigningKey = true, // Validates the key used to sign the token.
					NameClaimType = ClaimTypes.Name,
					IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(signingKey)
							// das hier als nächstes
					), // Creates the security key from the key specified in the configuration.
					
					ValidateLifetime = true, // <-- Token-Ablaufprüfung aktivieren
					ClockSkew = TimeSpan.Zero
			};
	});

// Repositories
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>(); 
builder.Services.AddScoped<ISdgTargetRepository, SdgTargetRepository>(); 
builder.Services.AddScoped<ITokenService, TokenService>(); 
builder.Services.AddScoped<ISdgTargetSubGoalRepository, SdgTargetSubGoalRepository>(); 
builder.Services.AddScoped<ISdgTargetSdgTypeRepository, SdgTargetSdgTypeRepository>(); 
builder.Services.AddScoped<ISdgTypeRepository, SdgTypeRepository>(); 
builder.Services.AddScoped<IIndustryRepository, IndustryRepository>(); 




// CORS 
builder.Services.AddCors(options =>
{
	options.AddPolicy("MyCorsPolicy", policy =>
	{
		var env = builder.Environment;
		if(env.IsDevelopment()){
			policy.WithOrigins("https://localhost:5173")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		} else if (env.EnvironmentName == "Staging" || env.EnvironmentName == "Test") {
			policy.WithOrigins("https://maive.local")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		} else {
			policy.WithOrigins("https://maive.f2.htw-berlin.de")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials(); 
		}
	});
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
															Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
	Console.WriteLine(">>> Umgebung: " + builder.Environment.EnvironmentName);
	Console.WriteLine(">>> ConnectionString: " + (connStr ?? "NULL"));

	try{
		// Migrations 
		var dbContext = services.GetRequiredService<ApplicationDBContext>(); 
		dbContext.Database.Migrate(); 

		// Initializations
		var configuration = services.GetRequiredService<IConfiguration>();
		await DbInitializer.InitializeRolesAndUsersAsync(services, configuration);
	}	catch (Exception ex)
    {
			// Logging oder Fehlerausgabe – hier einfach Konsole
			Console.WriteLine("Fehler beim Anwenden von Migrationen oder Seeding:");
			Console.WriteLine(ex.Message);
			throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseForwardedHeaders();
app.UseCors("MyCorsPolicy");

app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.None,
	Secure = CookieSecurePolicy.Always
});



app.UseAuthentication(); 
app.UseAuthorization(); 
// without that swagger will not work? 
app.MapControllers(); 

app.Run();
