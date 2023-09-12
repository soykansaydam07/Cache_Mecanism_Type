using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> Set(string name,string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                SlidingExpiration  = TimeSpan.FromSeconds(10)
            });
            //await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname));

            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname) , options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });

            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var namebinary = await _distributedCache.GetAsync("name");

            var surname = Encoding.UTF8.GetString(namebinary);
            return Ok(new 
            {
                name,
                surname
            });
        }

    }
}
