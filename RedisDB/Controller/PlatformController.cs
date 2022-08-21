using Microsoft.AspNetCore.Mvc;
using RedisDB.Data;
using RedisDB.Models;

namespace RedisDB.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {

        private readonly IPlatformRepo _repo;

        public PlatformController(IPlatformRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<Platform> GetPlatformById([FromRoute] string id)
        {
            var platform = _repo.GetPlatformById(id);

            if (platform != null)
            {
                return Ok(platform);
            }

            return NotFound();
        }

        [HttpPost(Name = "CreatePlatform")]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _repo.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platform.Id }, platform);
        }

        [HttpGet(Name = "GetAllPlatforms")]
        public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
        {
            var platforms = _repo.GetAllPlatforms();

            return Ok(platforms);
        }
    }
}
