using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Repository;
using Microsoft.AspNetCore.SignalR;

namespace DEVAMEET_CSharp.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomRepository _roomRepository;

        private string ClientId => Context.ConnectionId; //Criação do Id do cliente dentro do WebSocket (SignalR)

        public RoomHub(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public override async Task OnConnectedAsync() // entrada do cliente no hub e a geraão do ClientId
        {
            Console.WriteLine("Client:" + ClientId + " está conectado");
            await base.OnConnectedAsync();
        }

        public async Task Join(JoinDto joinDto)
        {
            var link = joinDto.Link;
            var userId = joinDto.UserId;

            Console.WriteLine("O usuario" + userId.ToString() + " está se juntando a sala com o ClientID:" + ClientId + " através do link" + link);

            await Groups.AddToGroupAsync(ClientId, link);

            var updatePositionDto = new UpdatePositionDto();

            updatePositionDto.X = 2;
            updatePositionDto.Y = 2;
            updatePositionDto.Orientation = "down";

            await _roomRepository.UpdateUserPosition(userId, link, ClientId, updatePositionDto);

            var users = await _roomRepository.ListUsersPosition(link);

            Console.WriteLine("Estamos enviando os usuarios para atualização");

            await Clients.Group(link).SendAsync("update-user-list", new { Users = users });
            await Clients.OthersInGroup(link).SendAsync("add-user", new { User = ClientId });

        }

    }
}
