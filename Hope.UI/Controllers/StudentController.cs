using Hope.Infrastructure.Base;
using Hope.Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Hope.UI.Controllers
{
	public class StudentController : BaseController
    {
		public async Task<IActionResult> Create()
		{
			HttpClient client = new HttpClient();
			var response = await client.GetAsync("http://localhost:5205/api/Student/GetListsToCreateStudent");

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				string apiResponse = response.Content.ReadAsStringAsync().Result;

				Infrastructure.DTO.CustomListDTO result = JsonConvert.DeserializeObject<Infrastructure.DTO.CustomListDTO>(apiResponse);

				ViewBag.Majors = result.ListDataMajors.ToList();
				ViewBag.StudyType = result.ListStudyType.ToList();
				ViewBag.TawjihiCertificate = result.ListTawjihiCertificate.ToList();
				ViewBag.Users = result.ListUsers.ToList();

			}
			else
			{
				// Error
			}
			return View();
		}

		public async Task<IActionResult> AddNewStudent(Infrastructure.DTO.StudentDTO studentDTO)
		{
            HttpClient client = new HttpClient();

            var ClientContextDTO = JsonConvert.SerializeObject(studentDTO);

            var response = await client.PostAsync("http://localhost:5205/api/Student/AddNewStudent",
                new StringContent(ClientContextDTO, Encoding.UTF8, "application/json"));

			return RedirectToAction("GetAllStudent");
        }


		public async Task<IActionResult> GetAllStudent()
		{
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("http://localhost:5205/api/Student/GetAllStudent");
            string apiResponse = await response.Content.ReadAsStringAsync();

            List<Infrastructure.DTO.StudentDTO> lst = new List<Infrastructure.DTO.StudentDTO>();
            lst = JsonConvert.DeserializeObject<List<Infrastructure.DTO.StudentDTO>>(apiResponse);

            return View(lst);
        }

    }
}
