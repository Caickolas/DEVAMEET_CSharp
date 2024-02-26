using DEVAMEET_CSharp.Models;

namespace DEVAMEET_CSharp.Repository
{
    public interface IMeetRepository
    {
        void CreateMeet(Meet meet);
        void DeleteMeet(int meetId);
        Meet GetMeetsById(int meetId);
        List<Meet> GetMeetsByUser(int iduser);
        void UpdateMeet(Meet meet);
    }
}
