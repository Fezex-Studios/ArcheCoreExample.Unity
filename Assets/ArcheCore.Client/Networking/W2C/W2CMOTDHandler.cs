using ArcheCore.Client.Networking;
using LiteNetLib;
using MessagePack;
using MMO.Shared.Packets;
using UnityEngine;

namespace MMO.Client.Networking.W2C.Handlers
{
    public class W2CMOTDHandler
        : IClientPacketHandler
    {
        public void Handle(
            NetPacketReader reader)
        {
            MOTDPacket packet =
                MessagePackSerializer
                    .Deserialize<MOTDPacket>(
                        reader.GetRemainingBytes());

            Debug.Log(
                $"MOTD: {packet.Message}");
        }
    }
}