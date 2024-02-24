using Microsoft.AspNetCore.Identity;

namespace DEVAMEET_CSharp.Dto
{
    public class LoginRequestDto
    {
        public string Login { get; set; }
        public string Password { get; set; } 
    }
}
