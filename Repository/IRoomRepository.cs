using DEVAMEET_CSharp.Dto;

namespace DEVAMEET_CSharp.Repository
{
    public interface IRoomRepository
    {
        Task<ICollection<PositionDto>> ListUsersPosition(string link);
        Task UpdateUserPosition(int userId, string link, string clientId, UpdatePositionDto updatePositionDto);
    }
}
