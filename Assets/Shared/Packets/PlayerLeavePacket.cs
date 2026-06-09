using MessagePack;

namespace Shared.Components
{
    [MessagePackObject(true)]
    public class PlayerLeavePacket
    {
        public int NetworkId;
    }
}