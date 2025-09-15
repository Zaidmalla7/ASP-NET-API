//using Hope.DomainEntities.DBEntities;
//using Hope.Repositories;
using Hope.Infrastructure.Base;
using Hope.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Hope.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
       // private readonly IRepository<Major> _major;

        public HomeController(ILogger<HomeController> logger)//, IRepository<Major> major)
        {
            _logger = logger;
           // _major = major;
        }

        public IActionResult Index()
        {
            //var result = _major.GetAll();
            return View();
        }

        public IActionResult ErrorPage()
        {
     
            return View();
        }

        public async Task<IActionResult> ErrorDetails()
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("http://localhost:5205/api/ErrorLog/GetAllErrors");
            string apiResponse = await response.Content.ReadAsStringAsync();

            List<Infrastructure.DTO.ErrorLogDTO> lst = new List<Infrastructure.DTO.ErrorLogDTO>();
            lst = JsonConvert.DeserializeObject<List<Infrastructure.DTO.ErrorLogDTO>>(apiResponse);

            return View(lst);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
