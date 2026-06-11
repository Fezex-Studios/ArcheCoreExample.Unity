using MessagePack;

namespace Shared.Packets
{
    [MessagePackObject(true)]
    public class AnnouncementPacket
    {
        public string Message;
    }
}