using DEVAMEET_CSharp.Dto;
using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using Microsoft.AspNetCore.SignalR;

namespace DEVAMEET_CSharp.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomRepository _roomRepository;

        public RoomHub(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        private string ClientId => Context.ConnectionId; //Criação do Id do cliente dentro do WebSocket (SignalR)

        public override async Task OnConnectedAsync() // entrada do cliente no hub e a geraão do ClientId
        {
            Console.WriteLine("Client:" + ClientId + " está conectado");
            await base.OnConnectedAsync();
        }

        public async Task Join(JoinDto joinDto)
        {
            var link = joinDto.Link;
            var userId = Int32.Parse(joinDto.UserId);

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
            
            Console.WriteLine("Mensagens enviadas referente a entrada de um novo usuario");

        }

        public async Task Move(MoveDto moveDto)
        {
            var userId = Int32.Parse(moveDto.UserId);
            var link = moveDto.Link;

            var updatePositionDto = new UpdatePositionDto();

            updatePositionDto.X = moveDto.X;
            updatePositionDto.Y = moveDto.Y;
            updatePositionDto.Orientation = moveDto.Orientation;

            await _roomRepository.UpdateUserPosition(userId, link, ClientId, updatePositionDto);

            var users = await _roomRepository.ListUsersPosition(link);

            Console.WriteLine("Estamos enviando a nova movimentação para todos os usuarios");

            await Clients.Group(link).SendAsync("update-user-list", new { Users = users });
        }

        public async Task UpdateUserMute (MuteDto muteDto)
        {
            var link = muteDto.Link;

            await _roomRepository.UpdateUserMute(muteDto);

            var users = await _roomRepository.ListUsersPosition(link);

            Console.WriteLine("Estamos enviando a nova movimentação para todos os usuarios");

            await Clients.Group(link).SendAsync("update-user-list", new { Users = users });
        }

        public async Task UpdateUserStatus(StatusDto statusDto)
        {
            var link = statusDto.Link;

            await _roomRepository.UpdateUserStatus(statusDto);

            var users = await _roomRepository.ListUsersPosition(link);

            Console.WriteLine("Estamos enviando a nova movimentação para todos os usuarios");

            await Clients.Group(link).SendAsync("update-user-list", new { Users = users });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("Disconnecting client!....");

            await _roomRepository.DeleteUserPosition(ClientId);

            await Clients.Others.SendAsync("remove-user", new { SocketId = ClientId });

            await base.OnDisconnectedAsync(exception);
        }

        public async Task CallUser (CallUserDto callUserDto)
        {
            await Clients.Client(callUserDto.To).SendAsync("call-made", new { Offer = callUserDto.Offer, Socket = ClientId });
        }

        public async Task MakeAnswer (MakeAnswerDto makeAnswerDto)
        {
            await Clients.Client(makeAnswerDto.To).SendAsync("answer-made", new { Answer = makeAnswerDto.Answer, Socket = ClientId });
        }

    }
}
