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

        public void CreateMeet(Meet meet)
        {
            _context.Meets.Add(meet);
            _context.SaveChanges();
        }

        public void DeleteMeet(int meetId)
        {
            List<MeetObjects> meetObjectsExist = _context.MeetObjects.Where(o => o.MeetId == meetId).ToList();

            foreach (MeetObjects meetObj in meetObjectsExist)
            {
                _context.MeetObjects.Remove(meetObj);
                _context.SaveChanges();
            }

            Meet meet = _context.Meets.FirstOrDefault(m => m.Id == meetId);

            _context.Meets.Remove(meet);
            _context.SaveChanges();
        }

        public Meet GetMeetsById(int meetId)
        {
            return _context.Meets.FirstOrDefault(m => m.Id == meetId);
        }

        public List<Meet> GetMeetsByUser(int iduser)
        {
            List<Meet> meets = _context.Meets.Where(m => m.UserId == iduser).ToList();

            foreach (Meet meet in meets)
            {
                meet.MeetObjects = _context.MeetObjects.Where(m => m.MeetId == meet.Id).ToList();
            }

            return meets;
        }

        public void UpdateMeet(Meet meet)
        {
            _context.Meets.Update(meet);
            _context.SaveChanges();
        }

    }
}
