using DEVAMEET_CSharp.Models;

namespace DEVAMEET_CSharp.Repository
{
    public interface IMeetObjectsRepository
    {
        void CreateObjectsMeet(List<MeetObjects> meetObjectsNew, int meetId);
    }
}
