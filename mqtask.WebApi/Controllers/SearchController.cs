using Microsoft.AspNetCore.Mvc;
using mqtask.Application;
using mqtask.Domain;

namespace mqtask.WebApi.Controllers
{
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        [Route("ip/location")]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Any, Duration = 3600)]
        public ActionResult GetLocationsByIp([FromQuery(Name = "ip")] string ip)
        {
            var result = LocationByIpFinder.Find(DbSnapshotHolder.Instance, ip);
            return Content(result, "application/json");
        }

        [HttpGet]
        [Route("city/locations")]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Any, Duration = 3600)]
        public ActionResult GetLocationsByCity([FromQuery(Name = "city")] string city)
        {
            var result = LocationsByCityFinder.Find(DbSnapshotHolder.Instance, city);
            return Content(result, "application/json");
        }
    }
}
