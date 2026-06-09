using MessagePack;

namespace Shared.Components
{
    [MessagePackObject(true)]
    public class PlayerPositionPacket
    {
        public int NetworkId;

        public float x;
        public float y;
        public float z;
    }
}