using System.Threading.Tasks;

namespace Lection_2_Core
{
    public interface IServerHub
    {
        Task SendMessage(string user, string message);
    }
}