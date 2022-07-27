using System.Threading;

namespace Lection_2_BL.ChatEntities
{
    public class Room
    {
        public string ReaderConnectionId { get; set; }
        public CancellationTokenSource ReaderCancellationTokenSource { get; set; }
        public string LibrarianConnectionId { get; set; }
        public CancellationTokenSource LibrarianCancellationTokenSource { get; set; }
    }
}
