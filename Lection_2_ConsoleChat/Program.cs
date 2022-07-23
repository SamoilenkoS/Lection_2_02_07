using Lection_2_Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
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

            await connection.StartAsync();
            Console.Write("Enter your name:");
            string username = Console.ReadLine();
            string message;
            do
            {
                Console.Write("Enter your message:");
                message = Console.ReadLine();
                await connection.InvokeAsync(
                    nameof(IServerHub.SendMessage),
                    username,
                    message);
            } while (!string.IsNullOrEmpty(message));

        }
    }
}
