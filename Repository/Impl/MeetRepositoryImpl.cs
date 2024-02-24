using DEVAMEET_CSharp.Models;

namespace DEVAMEET_CSharp.Repository.Impl
{
    public class MeetRepositoryImpl : IMeetRepository
    {
        private readonly DevameetContext _context;
        public MeetRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }


        public List<Meet> GetMeetsByUser(int iduser)
        {
            return _context.Meets.Where(m => m.UserId == iduser).ToList();
        }
    }
}
