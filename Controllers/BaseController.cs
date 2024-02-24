using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DEVAMEET_CSharp.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IUserRepository _userRepository;

        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected User GetToken()
        {
            var iduser = User.Claims.Where(c => c.Type == ClaimTypes.Sid ).Select(c => c.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(iduser))
            {
                return null;
            }
            else
            {
                return _userRepository.GetUserByLogin(int.Parse(iduser));
            }
        }
    }
}
