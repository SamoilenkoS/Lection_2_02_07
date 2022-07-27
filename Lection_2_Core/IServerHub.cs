using System.Threading.Tasks;

namespace Lection_2_Core
{
    public interface IServerHub
    {
        Task SendMessage(string message);
        Task<bool> SignIn(string login, string password);
        Task UserStartTyping();
    }
}