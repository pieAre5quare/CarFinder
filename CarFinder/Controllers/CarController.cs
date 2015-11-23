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
        /// Parameter object to recieve car filters.
        /// </summary>
        public class ControllerParams
        {
            /// <summary>
            /// Year of car to search for.
            /// </summary>
            public string year { get; set; }
            /// <summary>
            /// Make of car to search for.
            /// </summary>
            public string make { get; set; }
            /// <summary>
            /// Model of car to search for.
            /// </summary>
            public string model { get; set; }
            /// <summary>
            /// Trim of car to search for.
            /// </summary>
            public string trim { get; set; }
            /// <summary>
            /// Search string from user.
            /// </summary>
            public string filter { get; set; }
            /// <summary>
            /// If server side paging should be enabled.
            /// </summary>
            public bool paging { get; set; }
            /// <summary>
            /// Current page of cars.
            /// </summary>
            public int? page { get; set; }
            /// <summary>
            /// Number of cars to display on page.
            /// </summary>
            public int? perPage { get; set; }
            /// <summary>
            /// Column to sort by.
            /// </summary>
            public string sortcolumn { get; set; }
            /// <summary>
            /// If results should be ordered in reverse/descending.
            /// </summary>
            public bool sortByReverse { get; set; }

        }
        /// <summary>
        /// Wrapper object for the id, used when locating specific car.
        /// </summary>
        public class IdParam
        {
            /// <summary>
            /// Id of car to locate.
            /// </summary>
            public int Id { get; set; }
        }

        /// <summary>
        /// Gets all cars on the given year.
        /// </summary>
        /// <param name="selected">Parameter Object; only year variable is used.</param>
        /// <returns>A list of all cars in the database made on the given year.</returns>
        [Route("GetCarsByYear")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYear(ControllerParams selected)
        {
            return await db.GetCarsByYear(selected.year);
        }

        /// <summary>
        /// Gets all cars with given year make and model.
        /// </summary>
        /// <param name="selected">Parameter Object; only year, make, and model is used.</param>
        /// <returns>A list of all cars in the database with given year make and model</returns>
        [Route("GetCarsByYearMakeModel")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYearMakeModel(ControllerParams selected)
        {
            return await db.GetCarsByYearMakeModel(selected.year, selected.make, selected.model);
        }

        /// <summary>
        /// Gets all cars with given year make model and trim
        /// </summary>
        /// <param name="selected">Parameter Object; only year, make, model, and trim are used.</param>
        /// <returns>A list of cars with given year, make, model, and trim</returns>
        [Route("GetCarsByYearMakeModelTrim")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYearMakeModelTrim(ControllerParams selected)
        {
            return await db.GetCarsByYearMakeModelTrim(selected.year, selected.make, selected.model, selected.trim);
        }

        /// <summary>
        /// Gets all cars with given year and make.
        /// </summary>
        /// <param name="selected">Parameter Object; only year and make are used.</param>
        /// <returns>A list of all cars with given year and make.</returns>
        [Route("GetCarsByYearMake")]
        [HttpPost]
        public async Task<List<Car>> GetCarsByYearMake(ControllerParams selected)
        {
            return await db.GetCarsByYearMake(selected.year, selected.make);
        }

        /// <summary>
        /// Gets a list of makes by given year.
        /// </summary>
        /// <param name="selected">Parameter Object; only year is used.</param>
        /// <returns>A list of all makes on given year.</returns>
        [Route("GetMakeByYear")]
        [HttpPost]
        public async Task<List<string>> GetMakeByYear(ControllerParams selected)
        {
            return await db.GetMakeByYear(selected.year);
        }

        /// <summary>
        /// Gets a list of models with given year and make.
        /// </summary>
        /// <param name="selected">Parameter Object; only year and make are used.</param>
        /// <returns>A list of models with given year and make.</returns>
        [Route("GetModelsByYearMake")]
        [HttpPost]
        public async Task<List<string>> GetModelsByYearMake(ControllerParams selected)
        {
            return await db.GetModelsByYearMake(selected.year, selected.make);
        }

        /// <summary>
        /// Gets a list of trims with given year, make, and model.
        /// </summary>
        /// <param name="selected">Parameter Object; only year, make, and model are used.</param>
        /// <returns>A list of trims with given year, make, and model.</returns>
        [Route("GetTrimsByYearMakeModel")]
        [HttpPost]
        public async Task<List<string>> GetTrimsByYearMakeModel(ControllerParams selected)
        {
            return await db.GetTrimsByYearMakeModel(selected.year, selected.make, selected.model);
        }

        /// <summary>
        /// Gets all available years in database.
        /// </summary>
        /// <returns>A list of all available years.</returns>
        [Route("GetAllYears")]
        [HttpPost]
        public async Task<List<string>> GetAllYears()
        {
            return await db.GetAllYears();
        }

        /// <summary>
        /// Gets list of cars with given parameters, paging options can be set.
        /// </summary>
        /// <param name="selected">Parameter Object; 'ControllerParams' variables are used.</param>
        /// <returns>A list of cars with given options.</returns>
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
                                            selected.perPage, selected.sortcolumn, selected.sortByReverse);
        }


        /// <summary>
        /// Gets the number of cars with given year, make, model, trim, and search filter.
        /// </summary>
        /// <param name="selected">Paramter Object; only year, make, model, trim, filter are used.</param>
        /// <returns>The integer count of cars that match given options.</returns>
        [Route("GetCarsCount")]
        [HttpPost]
        public async Task<int> GetCarsCount(ControllerParams selected)
        {
            return await db.GetCarsCount(selected.year, selected.make, selected.model, selected.trim, selected.filter);
        }

        /// <summary>
        /// Get car with specified id.
        /// </summary>
        /// <param name="id">Id of car to be located.</param>
        /// <returns>Car with specified id, url of car image, and recalls from NHTSA.</returns>
        [Route("GetCar")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCar(IdParam id)
        {
            var car = db.Cars.Find(id.Id);
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
