using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

namespace InMemory.Caching.Controllers
{
    //In-Memory Cache İşlem Sırası :
    //AddMemoryCache servisini uygulamaya ekleyiniz
    //IMemoryCache referansını inject ediniz
    //Set metodu ile veri caclenip Get metodu ile cachelenmiş veri elde edebiliriz.
    //Remove fonsiyonu ile cahclenmiş veriyi silebiliriz
    //TryGetValue metodu ile kontrollü bir şekilde cacheden veriyi okuyabiliriz


    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("set/{name}")]
        public void SetName(string name)
        {
            _memoryCache.Set("name", name);
            
        }

        [HttpGet]
        public string GetName()
        {
            if (_memoryCache.TryGetValue<string>("name", out string name))
            {
                return _memoryCache.Get<string>("name");
            }

            return "";
        }

        [HttpGet("setDate")]
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("date", DateTime.UtcNow, options: new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(100),
                SlidingExpiration = TimeSpan.FromSeconds(15)
            });
        }
        [HttpGet("getDate")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("date");
        }

    }
}
