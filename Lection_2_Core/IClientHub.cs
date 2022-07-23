using System.Threading.Tasks;

namespace Lection_2_Core
{
    public interface IClientHub
    {
        Task ReceiveMessage(string user, string message);
    }
}