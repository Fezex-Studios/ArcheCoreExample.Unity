using LiteNetLib;
using MMO.Shared.Packets;
using Shared;
using Shared.Components;

namespace ArcheCore.WorldServer.Networking.W2C
{
    public static class W2CSpawnCubePacket
    {
        public static void Send(
            NetPeer peer,
            int     cubeId,
            float   x,
            float   y,
            float   z)
        {
            PacketSender.SendPacket(
                peer,
                PacketType.SpawnCube,
                new SpawnCubePacket
                {
                    CubeId = cubeId,
                    x      = x,
                    y      = y,
                    z      = z
                });
        }
    }
}