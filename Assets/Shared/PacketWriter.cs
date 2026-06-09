using LiteNetLib.Utils;

namespace Shared
{
    public static class PacketWriter
    {
        public static NetDataWriter Create(PacketType packet)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put((byte)packet);
            return writer;
        }
    }
}