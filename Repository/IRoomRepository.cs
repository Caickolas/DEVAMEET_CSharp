using DEVAMEET_CSharp.Dto;

namespace DEVAMEET_CSharp.Repository
{
    public interface IRoomRepository
    {
        Task DeleteUserPosition(string clientId);
        Task<ICollection<PositionDto>> ListUsersPosition(string link);
        Task UpdateUserMute(MuteDto muteDto);
        Task UpdateUserPosition(int userId, string link, string clientId, UpdatePositionDto updatePositionDto);
    }
}
