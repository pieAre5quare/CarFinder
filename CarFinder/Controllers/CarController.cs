using Bing;
using CarFinder.Models;
using Newtonsoft.Json;
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
        /// <summary>
        /// Gets all cars on the given year.
        /// </summary>
        /// <param name="year">Year to look up cars by</param>
        /// <returns>A list of all cars in the database made on the given year</returns>
        [Route("GetCarsByYear")]
        public async Task<List<Car>> GetCarsByYear(string year)
        {
            return await db.GetCarsByYear(year);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

        [Route("GetCar")]
        public async Task<IHttpActionResult> GetCar( int id)
        {
            var car = db.Cars.Find(id);
            if(car == null)
            {
                return await Task.FromResult(NotFound());
            }

            HttpResponseMessage response;

            var client = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));
            client.Credentials = new NetworkCredential("accountKey", "p378hNjOm0HKvheOJd/KFybFEM8wmO0orgleQPGPz9s");
            var marketData = client.Composite(
                "image",
                $"{car.model_year} {car.make} {car.model_name} {car.model_trim}",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                ).Execute();

            var imageUrl = marketData?.FirstOrDefault()?.Image?.FirstOrDefault()?.MediaUrl;

            dynamic recalls;

            using( var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://www.nhtsa.gov");

                try
                {
                    response = await httpClient.GetAsync($"webapi/api/Recalls/vehicle/modelyear/{car.model_year}/make/{car.make}/model/{car.model_name}?format=json");
                    recalls = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            return Ok(new { car, imageUrl, recalls });
        }
    }
}
