using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using mqtask.Application;
using mqtask.Application.Queries;
using mqtask.Domain.Entities;

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
            Location result = LocationByIpFinder.Find(DbSnapshotHolder.Instance, ip);
            return Ok(result);
        }

        [HttpGet]
        [Route("city/locations")]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Any, Duration = 3600)]
        public ActionResult GetLocationsByCity([FromQuery(Name = "city")] string city)
        {
            List<Location> result = LocationsByCityFinder.Find(DbSnapshotHolder.Instance, city);
            return Ok(result);
        }
    }
}
