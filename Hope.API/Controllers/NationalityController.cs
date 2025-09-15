using Hope.DomainEntities.DBEntities;
using Hope.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Buffers.Text;
using System;

namespace Hope.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class NationalityController : Controller
    {
        private readonly INationalityRepository _nationalityRepository;

        public NationalityController(INationalityRepository nationalityRepository)
        {
            _nationalityRepository = nationalityRepository;
        }

        public IActionResult GetAllNationality()
        {
            var list = _nationalityRepository.GetAll();

            string jsonString = JsonConvert.SerializeObject(list, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            return Ok(jsonString);
        }


        //If you build an API in . NET Core, it is called a Web API.
        //If your Web API follows REST principles (stateless, resource-based,
        //uses HTTP methods like GET, POST, PUT, DELETE), then it is also a REST API.

    }
}
