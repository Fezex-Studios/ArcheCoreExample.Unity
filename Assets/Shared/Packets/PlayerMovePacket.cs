using MessagePack;

namespace MMO.Shared.Packets
{
    [MessagePackObject(true)]
    public class PlayerMovePacket
    {
        public float x;
        public float y;
        public float z;
    }
}