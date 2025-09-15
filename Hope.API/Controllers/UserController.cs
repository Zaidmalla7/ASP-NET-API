using Hope.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Data.Common;
using System.Diagnostics;

namespace Hope.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INationalityRepository _nationalityRepository;
        Infrastructure.Helper.HopeErrorLog obj = new Infrastructure.Helper.HopeErrorLog();
        private readonly IAssignUserToRoleRepository _assignUserToRoleRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleRoleRepository _moduleRoleRepository;

        public UserController(IUserRepository userRepository, INationalityRepository nationalityRepository,
            IAssignUserToRoleRepository assignUserToRoleRepository, IModuleRepository moduleRepository,
            IModuleRoleRepository moduleRoleRepository)
        {
            _userRepository = userRepository;
            _nationalityRepository = nationalityRepository;
            _assignUserToRoleRepository = assignUserToRoleRepository;
            _moduleRepository = moduleRepository;
            _moduleRoleRepository = moduleRoleRepository;
        }

        public IActionResult AddNewUser(Hope.Infrastructure.DTO.UserDTO userDTO)
        {
            try
            {
                DomainEntities.DBEntities.User obj = new DomainEntities.DBEntities.User();

                obj.FirstName = userDTO.FirstName;
                obj.LastName = userDTO.LastName;
                obj.Email = userDTO.Email;
                obj.DateOfBirth = userDTO.DateOfBirth;
                obj.Mobile = userDTO.Mobile;
                obj.Gender = userDTO.Gender;
                obj.Address = userDTO.Address;
                obj.CreatedDate = DateTime.Now;
                obj.NationalityId = userDTO.NationalityId;

                _userRepository.Add(obj);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                obj.AddErrorLog(ex, "2", "User Module");
                return BadRequest("Error");
            }
        }

        public IActionResult GetAllUsers()
        {
            Infrastructure.Base.Security security = new Infrastructure.Base.Security();

            try
            {


                List<Infrastructure.DTO.UserDTO> lst = new List<Infrastructure.DTO.UserDTO>();

                lst = (from obj in _userRepository.Find(x => x.Id != 0, x => x.Nationality)

                       select new Infrastructure.DTO.UserDTO
                       {
                           UserId = obj.Id,
                           EncryptUserId = security.EncryptString(obj.Id.ToString()),
                           FirstName = obj.FirstName,
                           LastName = obj.LastName,
                           Email = obj.Email,
                           Address = obj.Address,
                           DateOfBirth = obj.DateOfBirth,
                           GenderDisplayName = obj.Gender == true ? "Male" : "Female",
                           Mobile = obj.Mobile,
                           NationalityName = obj.Nationality.Name,
                       }).ToList();

                string jsonString = JsonConvert.SerializeObject(lst, Formatting.None, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return Ok(jsonString);
            }
            catch (Exception ex)
            {
                obj.AddErrorLog(ex, "2", "User Module");
                return BadRequest("Error");
            }

        }

        public IActionResult DeleteUser(int UserId)
        {
            try
            {

                DomainEntities.DBEntities.User obj = new DomainEntities.DBEntities.User();
                obj = _userRepository.Find(x => x.Id == UserId).FirstOrDefault();

                _userRepository.Delete(obj);
                return Ok("Success");
            }
            catch (Exception ex)
            {

                obj.AddErrorLog(ex, "2", "User Module");
                return BadRequest("Error");
            }
        }

        public IActionResult GetUserById(string UserId)
        {
            try
            {
                Infrastructure.Base.Security security = new Infrastructure.Base.Security();
                int _UserId = Convert.ToInt32(security.DecryptString(UserId));

                Infrastructure.DTO.UserDTO userDTO = new Infrastructure.DTO.UserDTO();

                userDTO = (from obj in _userRepository.Find(x => x.Id == _UserId)
                           select new Infrastructure.DTO.UserDTO
                           {
                               UserId = obj.Id,
                               FirstName = obj.FirstName,
                               LastName = obj.LastName,
                               Mobile = obj.Mobile,
                               Address = obj.Address,
                               DateOfBirth = obj.DateOfBirth,
                               Email = obj.Email,
                               Gender = obj.Gender,
                               NationalityId = obj.NationalityId,
                           }).FirstOrDefault();

                return Ok(userDTO);

            }
            catch (Exception ex)
            {

                obj.AddErrorLog(ex, "2", "User Module");
                return BadRequest("Error");
            }

        }

        public IActionResult GetFullNameById(int UserId)
        {
            var result =  _userRepository.Find(x => x.Id == UserId).FirstOrDefault();
            return Ok(result.FirstName + " " + result.LastName);
        }

        public IActionResult UpdateUser(Hope.Infrastructure.DTO.UserDTO userDTO)
        {
            try
            {

            
            DomainEntities.DBEntities.User obj = new DomainEntities.DBEntities.User();

            obj = _userRepository.Find(x => x.Id == userDTO.UserId).FirstOrDefault();

            obj.FirstName = userDTO.FirstName;
            obj.LastName = userDTO.LastName;
            obj.Email = userDTO.Email;
            obj.DateOfBirth = userDTO.DateOfBirth;  
            obj.Address = userDTO.Address;
            obj.NationalityId = userDTO.NationalityId;
            obj.Mobile = userDTO.Mobile;
            obj.Gender = userDTO.Gender;

            _userRepository.Update(obj);

            return Ok("Ok");

            }
            catch (Exception ex)
            {

                obj.AddErrorLog(ex, "2", "User Module");
                return BadRequest("Error");
            }
        }

        public IActionResult Login(Infrastructure.DTO.LoginDTO loginDTO)
        {
            var result = _userRepository.Find(x => x.Username == loginDTO.UserName &&
            x.Password == loginDTO.Password).FirstOrDefault();

            if(result != null)
            {
                return Ok(result.Id);
            }
            else
            {
                return BadRequest(-1);
            }

            
        }

        [HttpGet]
        public IActionResult GetAllPermissionByUserId(int Id)
        {
            List<int> lstRoles = new List<int>();
            lstRoles = _assignUserToRoleRepository.Find(x => x.UserId == Id).Select(x => x.RoleId).ToList();


            List<int> lstModules = new List<int>();
            lstModules = _moduleRoleRepository.Find( x => lstRoles.Contains(x.RoleId)).Select(x=> x.ModuleId).Distinct().ToList();


            Infrastructure.DTO.MenuPermissionDTO menuPermissionDTO = new Infrastructure.DTO.MenuPermissionDTO();

            if (lstModules.Contains(1))
                menuPermissionDTO.User = "True";
            if (lstModules.Contains(2))
                menuPermissionDTO.Student = "True";
            if (lstModules.Contains(3))
                menuPermissionDTO.Role = "True";
            if (lstModules.Contains(4))
                menuPermissionDTO.AssingUserToRole = "True";


            return Ok(menuPermissionDTO);
        }
    }
}
