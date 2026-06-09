using MessagePack;

namespace MMO.Shared.Packets
{
    [MessagePackObject(true)]
    public class MOTDPacket
    {
        public string Message;
    }
}