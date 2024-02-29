using DEVAMEET_CSharp.Models;
using Microsoft.IdentityModel.Tokens;

namespace DEVAMEET_CSharp.Repository.Impl
{
    public class MeetObjectsRepositoryImpl : IMeetObjectsRepository
    {
        private readonly DevameetContext _context;

        public MeetObjectsRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }
        public void CreateObjectsMeet(List<MeetObjects> meetObjectsNew, int meetId)
        {
             List<MeetObjects> meetObjectsExist = _context.MeetObjects.Where(o => o.MeetId == meetId).ToList();

            foreach(MeetObjects meetObj in meetObjectsExist) 
            { 
                _context.MeetObjects.Remove(meetObj);
                _context.SaveChanges();
            }

            foreach(MeetObjects meetObj in meetObjectsNew)
            {
                _context.MeetObjects.Add(meetObj);
                _context.SaveChanges();
            }
        }

        public List<MeetObjects> GetObjectsByMeet(int meetid)
        {
            return _context.MeetObjects.Where(o => o.MeetId == meetid).ToList();
        }
    }
}
