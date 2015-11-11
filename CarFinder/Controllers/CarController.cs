using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarFinder.Controllers
{
    [RoutePrefix("api/Cars")]
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("GetCarsByYear")]
        public async Task<List<Car>> GetCarsByYear(string year)
        {
            return await db.GetCarsByYear(year);
        }

        [Route("GetCarsByYearMakeModel")]
        public async Task<List<Car>> GetCarsByYearMakeModel(string year, string make, string model)
        {
            return await db.GetCarsByYearMakeModel(year, make, model);
        }

        [Route("GetCarsByYearMakeModelTrim")]
        public async Task<List<Car>> GetCarsByYearMakeModelTrim(string year, string make, string model, string trim)
        {
            return await db.GetCarsByYearMakeModelTrim(year, make, model, trim);
        }

        [Route("GetCarsByYearMake")]
        public async Task<List<Car>> GetCarsByYearMake(string year, string make)
        {
            return await db.GetCarsByYearMake(year, make);
        }

        [Route("GetMakeByYear")]
        public async Task<List<string>> GetMakeByYear(string year)
        {
            return await db.GetMakeByYear(year);
        }

        [Route("GetModelsByYearMake")]
        public async Task<List<string>> GetModelsByYearMake(string year, string make)
        {
            return await db.GetModelsByYearMake(year, make);
        }

        [Route("GetTrimsByYearMakeModel")]
        public async Task<List<string>> GetTrimsByYearMakeModel(string year, string make, string model)
        {
            return await db.GetTrimsByYearMakeModel(year, make, model);
        }

        [Route("GetAllYears")]
        public async Task<List<string>> GetAllYears()
        {
            return await db.GetAllYears();
        }
    }
}
