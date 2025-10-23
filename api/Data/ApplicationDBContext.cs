using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    /**
     * @brief Represents the database context for the application.
     * 
     * @details Integrates Entity Framework Core and ASP.NET Core Identity
     * to manage database operations and user authentication.
     */
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        /**
         * @param options DbContext options for configuring the context.
         * @exception ArgumentNullException Thrown if options are null.
         */
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions ?? throw new ArgumentNullException(nameof(dbContextOptions)))
        {
            
        }

        // DbSet properties represent collections of entities that correspond to tables in the database.
        // Defining DbSet for SdgTargets and Companies allows the context to manage these entities, including
        // performing CRUD operations. This setup enables the use of LINQ queries on these entities, which
        // are automatically translated into queries against the database.
        public DbSet<SdgTarget> SdgTargets{ get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SdgTargetSubGoal> SdgTargetSubGoals {get; set;}
        public DbSet<SdgType> SdgTypes {get; set;}
        public DbSet<SdgTargetSdgType> SdgTargetSdgTypes {get; set;}
        public DbSet<Industry> Industries { get; set; }


        // is used to make specific configurations and insert standard data, such as user roles, into the database to ensure security from the outset 
        //and simplify the setup process after the initial installation.
        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder); 
            builder.Entity<SdgTargetSdgType>(x => x.HasKey(s => new{ s.SdgTargetId, s.SdgTypeId}));

            builder.Entity<SdgTargetSdgType>()
                .HasOne(s => s.SdgTarget)
                .WithMany(s => s.SdgTargetSdgTypes)
                .HasForeignKey(s => s.SdgTargetId)
                .OnDelete(DeleteBehavior.Cascade); // 🔹 Setzt CASCADE DELETE
            
            builder.Entity<SdgTargetSdgType>()
                .HasOne(s => s.SdgType)
                .WithMany(s => s.SdgTargetSdgTypes)
                .HasForeignKey(s => s.SdgTypeId)
                .OnDelete(DeleteBehavior.Restrict); // 🔹 Verhindert das Löschen von SDG-Typen
            
            //Cascade delete for Company and SdgTargets
            builder.Entity<SdgTarget>()
                .HasOne(s => s.Company)
                .WithMany(c => c.SdgTargets)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Cascade delete for other relationships if needed
            // Add similar configurations for other dependent entities if necessary. 
            
            
            //************************************************/
            //**************** Identity Roles ****************/
            //************************************************/

            // This section initializes a list of essential identity roles such as "Admin" and "User". 
            // to ensure necessary access controls are in place from launch.
            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Name = "Admin",
                    NormalizedName = "ADMIN" // Uppercase format to standardize role checks across the application.
                }, 
                new IdentityRole{
                    Name = "User",
                    NormalizedName = "USER" // Uppercase format to standardize role checks across the application.
                }, 

            }; 
            // Seeds the roles into the database to ensure they are available immediately after deployment,
            builder.Entity<IdentityRole>().HasData(roles); 
        }
        
    }
}