using DEVAMEET_CSharp.Models;

namespace DEVAMEET_CSharp.Repository
{
    public interface IMeetRepository
    {
        List<Meet> GetMeetsByUser(int iduser);
    }
}
