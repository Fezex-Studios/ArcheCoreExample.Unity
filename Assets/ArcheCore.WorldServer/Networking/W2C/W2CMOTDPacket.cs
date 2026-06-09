// Server/Networking/W2C/W2CMOTDPacket.cs
using LiteNetLib;
using MMO.Shared.Packets;
using Shared;

namespace ArcheCore.WorldServer.Networking.W2C
{
    public static class W2CMOTDPacket
    {
        public static void Send(
            NetPeer peer,
            string message)
        {
            PacketSender.SendPacket(
                peer,
                PacketType.MOTD,
                new MOTDPacket
                {
                    Message = message
                });
        }
    }
}