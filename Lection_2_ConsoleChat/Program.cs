using Lection_2_Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_ConsoleChat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection;
            connection = new HubConnectionBuilder()
              .WithUrl("https://localhost:5001/chat")
              .Build();
            connection.On<string, string>(
                nameof(IClientHub.ReceiveMessage), (user, message) =>
                {
                    Console.WriteLine($"{user}:{message}");
                });
            connection.On<bool>(
                nameof(IClientHub.RoomateTyping), (isTyping) =>
                {
                    if (isTyping)
                    {
                        //got message from screen
                        //clear it
                        //write "user is typing"
                        //write message from buffer
                        Console.WriteLine("Typing");
                    }
                    else
                    {
                        //got message from screen
                        //clear
                        //write message from buffer
                        Console.WriteLine("Stopped!");
                    }
                });

            await connection.StartAsync();
            StringBuilder messageBuilder = new StringBuilder(string.Empty);
            var needCleaning = false;
            do
            {
                if (needCleaning)
                {
                    messageBuilder = new StringBuilder(string.Empty);
                    needCleaning = false;
                }

                var key = Console.ReadKey();
                if(key.Key != ConsoleKey.Enter)
                {
                    if(key.Key != ConsoleKey.Backspace)
                    {
                        messageBuilder.Append(key.KeyChar);
                    }
                    else
                    {
                        messageBuilder.Remove(messageBuilder.Length - 1, 1);
                        var currentPosition = Console.GetCursorPosition();
                        Console.Write(' ');
                        Console.SetCursorPosition(currentPosition.Left, currentPosition.Top);
                    }

                    await connection.SendAsync(nameof(IServerHub.UserStartTyping)).ConfigureAwait(false);
                }
                else
                {
                    if(messageBuilder.Length > 0)
                    {
                        await SendMessageToClient
                            (connection, messageBuilder.ToString());
                        needCleaning = true;
                    }
                }

            } while (messageBuilder.Length > 0);

        }

        private static async Task SendMessageToClient(
            HubConnection connection,
            string message)
        {
            if (message.StartsWith('/'))//commands logic
            {
                string command = message.Substring(1, message.IndexOf(' ') - 1).ToLower();
                switch (command)
                {
                    case "signin":
                        var parameters = message
                            .Substring(
                                message.IndexOf(' ') + 1)
                            .Split(' ');
                        if (parameters.Length < 2)
                        {
                            break;
                        }
                        string login = parameters[0];
                        string password = string.Join(' ', parameters[1..]);
                        var response = await connection.InvokeAsync<bool>(
                            nameof(IServerHub.SignIn),
                            login, password);
                        break;
                }
            }
            else
            {
                await connection.InvokeAsync(
                    nameof(IServerHub.SendMessage),
                    message);
            }
        }
    }
}
