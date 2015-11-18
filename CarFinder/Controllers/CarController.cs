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

        public class ControllerParams
        {
            public string year { get; set; }
            public string make { get; set; }
            public string model { get; set; }
            public string trim { get; set; }
            public string filter { get; set; }
            public bool? paging { get; set; }
            public int? page { get; set; }
            public int? perPage { get; set; }
            public string sortcolumn { get; set; }
            public string sortdirection { get; set; }

        }

        public class IdParam
        {
            public int id { get; set; }
        }

        /// <summary>
        /// Gets all cars on the given year.
        /// </summary>
        /// <param name="year">Year to look up cars by</param>
        /// <returns>A list of all cars in the database made on the given year</returns>
        [Route("GetCarsByYear")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYear(ControllerParams selected)
        {
            return await db.GetCarsByYear(selected.year);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetCarsByYearMakeModel")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYearMakeModel(ControllerParams selected)
        {
            return await db.GetCarsByYearMakeModel(selected.year, selected.make, selected.model);
        }

        [Route("GetCarsByYearMakeModelTrim")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYearMakeModelTrim(ControllerParams selected)
        {
            return await db.GetCarsByYearMakeModelTrim(selected.year, selected.make, selected.model, selected.trim);
        }

        [Route("GetCarsByYearMake")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYearMake(ControllerParams selected)
        {
            return await db.GetCarsByYearMake(selected.year, selected.make);
        }

        [Route("GetMakeByYear")]
        [HttpPost]
        public async Task<List<string>> GetMakeByYear(ControllerParams selected)
        {
            return await db.GetMakeByYear(selected.year);
        }

        [Route("GetModelsByYearMake")]
        [HttpPost]
        public async Task<List<string>> GetModelsByYearMake(ControllerParams selected)
        {
            return await db.GetModelsByYearMake(selected.year, selected.make);
        }

        [Route("GetTrimsByYearMakeModel")]
        [HttpPost]
        public async Task<List<string>> GetTrimsByYearMakeModel(ControllerParams selected)
        {
            return await db.GetTrimsByYearMakeModel(selected.year, selected.make, selected.model);
        }

        [Route("GetAllYears")]
        [HttpPost]
        public async Task<List<string>> GetAllYears()
        {
            return await db.GetAllYears();
        }

        [Route("GetCars")]
        [HttpPost]
        public async Task<List<Car>> GetCars(ControllerParams selected)
        {
            if (selected == null)
            {
                selected = new ControllerParams()
                {
                    year = "2000"
                };
            }
            return await db.GetCars(selected.year, selected.make, selected.model, selected.trim, selected.filter, selected.paging, selected.page,
                                            selected.perPage, selected.sortcolumn, selected.sortdirection);
        }

        [Route("GetCar")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCar(IdParam paramId)
        {
            var car = db.Cars.Find(paramId.id);
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
