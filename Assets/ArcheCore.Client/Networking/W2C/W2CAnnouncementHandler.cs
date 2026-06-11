using LiteNetLib;
using MessagePack;
using Shared.Packets;
using UnityEngine;

namespace ArcheCore.Client.Networking.W2C
{
    public class W2CAnnouncementHandler : IClientPacketHandler
    {
        public void Handle(NetPacketReader reader)
        {
            AnnouncementPacket packet = MessagePackSerializer
                .Deserialize<AnnouncementPacket>(reader.GetRemainingBytes());
            
            Debug.Log($"Received announcement packet: {packet.Message}");
        }
    }
}