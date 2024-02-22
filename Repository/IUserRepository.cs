using DEVAMEET_CSharp.Models;

namespace DEVAMEET_CSharp.Repository
{
    public interface IUserRepository
    {
        User GetUserByLoginPassword(string login, string password);
        void Save(User user);
        bool VerifyEmail(string email);
        User GetUserByLogin(int iduser);
    }
}
