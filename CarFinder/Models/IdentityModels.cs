using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarFinder.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Car> Cars { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public async Task<List<Car>> GetCarsByYear(string year)
        {
            var yearParam = new SqlParameter("@model_year", year);

            return await this.Database
                .SqlQuery<Car>("GetCarsByYear @model_year", yearParam).ToListAsync();
        }

        public async Task<List<Car>> GetCarsByYearMakeModel(string year, string make, string model)
        {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make_name", make);
            var modelParam = new SqlParameter("@model_name", model);

            return await this.Database
                .SqlQuery<Car>("GetCarsByYearMakeModel @model_year, @make_name, @model_name", yearParam, makeParam, modelParam).ToListAsync();
        }

        public async Task<List<Car>> GetCarsByYearMakeModelTrim(string year, string make, string model, string trim)
        {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make_name", make);
            var modelParam = new SqlParameter("@model_name", model);
            var trimParam = new SqlParameter("@model_trim", trim);

            return await this.Database
                .SqlQuery<Car>("GetCarsByYearMakeModelTrim @model_year, @make_name, @model_name, @model_trim", yearParam, makeParam, modelParam, trimParam).ToListAsync();
        }

        public async Task<List<Car>> GetCarsByYearMake(string year, string make)
        {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make_name", make);

            return await this.Database
                .SqlQuery<Car>("GetCarsYearAndMake @model_year, @make_name", yearParam, makeParam).ToListAsync();
        }

        public async Task<List<string>> GetMakeByYear(string year)
        {
            var yearParam = new SqlParameter("@model_year", year);

            return await this.Database
                .SqlQuery<string>("GetMakeByYear @model_year", yearParam).ToListAsync();
        }

        public async Task<List<string>> GetModelsByYearMake(string year, string make)
        {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make_name", make);

            return await this.Database
                .SqlQuery<string>("GetModelsByYearMake @model_year, @make_name", yearParam, makeParam).ToListAsync();
        }

        public async Task<List<string>> GetTrimsByYearMakeModel(string year, string make, string model)
        {
            var yearParam = new SqlParameter("@model_year", year);
            var makeParam = new SqlParameter("@make_name", make);
            var modelParam = new SqlParameter("@model_name", model);

            return await this.Database
                .SqlQuery<string>("GetTrims @model_year, @make_name, @model_name", yearParam, makeParam, modelParam).ToListAsync();
        }

        public async Task<List<string>> GetAllYears()
        {
            return await this.Database
                .SqlQuery<string>("GetYears").ToListAsync();
        }

        public async Task<List<Car>> GetCars(string year, string make, string model, string trim, string filter, bool? paging, int? page, 
                                            int? perPage, string sortcolumn, string sortdirection )
        {
            var yearParam = new SqlParameter("@year", year);
            var makeParam = new SqlParameter("@make", make ?? "");
            var modelParam = new SqlParameter("@model", model ?? "");
            var trimParam = new SqlParameter("@trim", trim ?? "");
            var filterParam = new SqlParameter("@filter", filter ?? "");
            var pagingParam = new SqlParameter("@paging", paging ?? false);
            var pageParam = new SqlParameter("@page", page ?? 1);
            var perPageParam = new SqlParameter("@perPage", perPage ?? 50);
            var sortcolumnParam = new SqlParameter("@sortcolumn", sortcolumn ?? "");
            var sortdirectionParam = new SqlParameter("@sortdirection", sortdirection ?? "");

            return await this.Database.SqlQuery<Car>("GetCars @year, @make, @model, @trim, @filter, @paging, @page, @perPage, @sortcolumn, @sortdirection",
                                yearParam, makeParam, modelParam, trimParam, filterParam, pagingParam, pageParam, perPageParam, sortcolumnParam, sortdirectionParam).ToListAsync();
        }
    }
}