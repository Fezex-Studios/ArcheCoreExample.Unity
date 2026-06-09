using ArcheCore.Client.Gameplay;
using LiteNetLib;
using MessagePack;
using Shared.Components;
using UnityEngine;

namespace ArcheCore.Client.Networking.W2C
{
    public class W2CPlayerPositionHandler
        : IClientPacketHandler
    {
        public void Handle(NetPacketReader reader)
        {
            PlayerPositionPacket packet =
                MessagePackSerializer
                    .Deserialize<PlayerPositionPacket>(
                        reader.GetRemainingBytes());

            if (packet.NetworkId ==
                ClientNetwork.Instance.LocalNetworkId)
            {
                return;
            }

            PlayerRegistry.Instance
                ?.UpdatePosition(
                    packet.NetworkId,
                    new Vector3(
                        packet.x,
                        packet.y,
                        packet.z));
        }
    }
}