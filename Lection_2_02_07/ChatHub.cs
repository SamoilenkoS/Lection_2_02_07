using Lection_2_BL;
using Lection_2_BL.Auth;
using Lection_2_BL.ChatEntities;
using Lection_2_BL.Services.AuthService;
using Lection_2_Core;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Lection_2_02_07
{
    public class ChatHub : Hub<IClientHub>, IServerHub
    {
        private static List<Room> _chatRooms;
        private readonly IAuthService _authService;
        private readonly ITokenGenerator _tokenGenerator;
        private Func<Room, bool> ByReader = x => string.IsNullOrEmpty(x.ReaderConnectionId);
        private Func<Room, bool> ByLibrarian = x => string.IsNullOrEmpty(x.LibrarianConnectionId);
        private int _delayInSeconds = 5;

        static ChatHub()
        {
            _chatRooms = new List<Room>();
        }

        public ChatHub(
            IAuthService authService,
            ITokenGenerator tokenGenerator)
        {
            _authService = authService;
            _tokenGenerator = tokenGenerator;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Others.ReceiveMessage(Context.ConnectionId, " has been connected!");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.ReceiveMessage(Context.ConnectionId, " has been disconnected!");
        }

        public async Task SendMessage(string message)
        {
            var room = _chatRooms.FirstOrDefault(x =>
                x.LibrarianConnectionId == Context.ConnectionId
                || x.ReaderConnectionId == Context.ConnectionId);
            if(room != null)
            {
                var role = room.ReaderConnectionId == Context.ConnectionId
                    ? Roles.Reader
                    : Roles.Librarian;
                var targetId = room.ReaderConnectionId == Context.ConnectionId
                    ? room.LibrarianConnectionId
                    : room.ReaderConnectionId;

                await Clients.Client(targetId).ReceiveMessage(role, message);
            }
        }

        public async Task<bool> SignIn(string login, string password)
        {
            try
            {
                var token = await _authService.SignIn(login, password);
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                var role = _tokenGenerator.GetClaimValueFromToken(
                    token,
                    ClaimsIdentity.DefaultRoleClaimType.ToString());
                var predicate = role == Roles.Reader ? ByReader : ByLibrarian;
                var room = _chatRooms.FirstOrDefault(predicate);
                if(room == null)
                {
                    room = new Room();
                    _chatRooms.Add(room);
                }

                if(role == Roles.Reader)
                {
                    room.ReaderConnectionId = Context.ConnectionId;
                }
                else if(role == Roles.Librarian)
                {
                    room.LibrarianConnectionId = Context.ConnectionId;
                }
                if(!string.IsNullOrEmpty(room.ReaderConnectionId)
                    && !string.IsNullOrEmpty(room.LibrarianConnectionId))
                {
                    await Clients.Clients(new List<string> {room.ReaderConnectionId, room.LibrarianConnectionId })
                        .ReceiveMessage("SYSTEM", "You're added to chat room!");
                }
                else
                {
                    await Clients.Clients(new List<string> { room.ReaderConnectionId, room.LibrarianConnectionId })
                        .ReceiveMessage("SYSTEM", "You're added to waiting list!");
                }
                //TODO add notification about room filling (if yes)
                //TODO send message only to user in your room
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public async Task UserStartTyping()
        {
            var room = _chatRooms.FirstOrDefault(x =>
                  x.LibrarianConnectionId == Context.ConnectionId
                  || x.ReaderConnectionId == Context.ConnectionId);
            if(room != null)
            {
                var role = room.ReaderConnectionId == Context.ConnectionId
                  ? Roles.Reader
                  : Roles.Librarian;
                var targetId = room.ReaderConnectionId == Context.ConnectionId
                    ? room.LibrarianConnectionId
                    : room.ReaderConnectionId;
                await Clients.Client(targetId).RoomateTyping(true);
                var tokenSource = room.ReaderConnectionId == Context.ConnectionId
                    ? room.LibrarianCancellationTokenSource
                    : room.ReaderCancellationTokenSource;
                if(tokenSource != null)
                {
                    tokenSource.Cancel();
                    await UserStopTyping(targetId, tokenSource.Token).ConfigureAwait(false);
                }
                else
                {
                    if(room.ReaderConnectionId == Context.ConnectionId)//reader
                    {
                        room.ReaderCancellationTokenSource = new CancellationTokenSource();
                        await UserStopTyping(targetId,
                            room.ReaderCancellationTokenSource.Token);
                    }
                    else
                    {
                        room.LibrarianCancellationTokenSource = new CancellationTokenSource();
                        await UserStopTyping(targetId,
                            room.LibrarianCancellationTokenSource.Token);
                    }
                }
            }
        }

        private async Task UserStopTyping(string clientId, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(_delayInSeconds));
            if (!cancellationToken.IsCancellationRequested)
            {
                await Clients.Client(clientId).RoomateTyping(false);
            }
        }
    }
}
