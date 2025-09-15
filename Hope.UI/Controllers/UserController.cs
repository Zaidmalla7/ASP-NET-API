using Hope.Infrastructure.Base;
using Hope.Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


namespace Hope.UI.Controllers
{
    public class UserController : BaseController
    {
        public async Task<IActionResult> Create()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5205/api/Nationality/GetAllnationality");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;

                if (apiResponse == "Error")
                {
                    return RedirectToAction("ErrorPage", "Home");
                }

                List<Hope.Infrastructure.DTO.NationalityDTO> lst = new List<Infrastructure.DTO.NationalityDTO>();

                lst = JsonConvert.DeserializeObject<List<Infrastructure.DTO.NationalityDTO>>(apiResponse);
                ViewBag.Nationality = lst;
            }


            var responseDep = await client.GetAsync("http://localhost:5205/api/Department/GetAllDepartment");

            if (responseDep.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string apiResponse = responseDep.Content.ReadAsStringAsync().Result;

                if (apiResponse == "Error")
                {
                    return RedirectToAction("ErrorPage", "Home");
                }

                List<Hope.Infrastructure.DTO.DepartmentDTO> lst = new List<Infrastructure.DTO.DepartmentDTO>();

                lst = JsonConvert.DeserializeObject<List<Infrastructure.DTO.DepartmentDTO>>(apiResponse);
                ViewBag.Department = lst;
            }
            return View();
        }

        public async Task<IActionResult> Update(string Id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5205/api/User/GetUserById?UserId=" + Id);

            var apiResponse = await response.Content.ReadAsStringAsync();

            if (apiResponse == "Error")
            {
                return RedirectToAction("ErrorPage", "Home");
            }


            Infrastructure.DTO.UserDTO userDTO = JsonConvert.DeserializeObject<Infrastructure.DTO.UserDTO>(apiResponse);

           

            var responseNat = await client.GetAsync("http://localhost:5205/api/Nationality/GetAllnationality");
            string apiResponseNat = responseNat.Content.ReadAsStringAsync().Result;
            List<Hope.Infrastructure.DTO.NationalityDTO> lst = new List<Infrastructure.DTO.NationalityDTO>();
            lst = JsonConvert.DeserializeObject<List<Infrastructure.DTO.NationalityDTO>>(apiResponseNat);
            ViewBag.Nationality = lst;


            return View(userDTO);
        }


        public async Task<IActionResult> UpdateUser(Hope.Infrastructure.DTO.UserDTO userDTO) 
        {

            HttpClient client = new HttpClient();

            var ClientContextDTO = JsonConvert.SerializeObject(userDTO);

            var response = await client.PostAsync("http://localhost:5205/api/User/UpdateUser",
                new StringContent(ClientContextDTO, Encoding.UTF8, "application/json"));

            return RedirectToAction("GetAllUsers");
        }

        public async Task<IActionResult> AddNewUser(Hope.Infrastructure.DTO.UserDTO userDTO)
        {
            HttpClient client = new HttpClient();

            var ClientContextDTO = JsonConvert.SerializeObject(userDTO);

            var response = await client.PostAsync("http://localhost:5205/api/User/AddNewUser",
                new StringContent(ClientContextDTO, Encoding.UTF8, "application/json"));
           
            //if (response == "Error")
            //{
            //    return RedirectToAction("ErrorPage", "Home");
            //}

            return View();
        }

        public async Task<IActionResult> GetAllUsers()
        {
            
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("http://localhost:5205/api/User/GetAllUsers");
            string apiResponse = await response.Content.ReadAsStringAsync();

            if(apiResponse == "Error")
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            List<Infrastructure.DTO.UserDTO> lst = new List<Infrastructure.DTO.UserDTO>();
            lst = JsonConvert.DeserializeObject<List<Infrastructure.DTO.UserDTO>>(apiResponse);

            return View(lst);
        }


        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5205/api/User/DeleteUser?UserId=" + id);

            //if (response = "Error")
            //{
            //    return RedirectToAction("ErrorPage", "Home");
            //}


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View("Ok");
            }
            else
            {
                return View();
            }

           
        }

        public async Task<IActionResult> GetAllSectionByDeptId(string DeptId)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("http://localhost:5205/api/Department/GetAllSectionsByDepartmentId?DepartmentId=" + DeptId);

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<Infrastructure.DTO.SectionDTO> lst = JsonConvert.DeserializeObject<List<Hope.Infrastructure.DTO.SectionDTO>>(apiResponse);

            return Json(lst);
        }
    }
}
