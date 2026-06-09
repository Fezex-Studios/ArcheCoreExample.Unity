using ArcheCore.Client.Gameplay;
using LiteNetLib;
using MessagePack;
using Shared.Components;

namespace ArcheCore.Client.Networking.W2C
{
    public class W2CPlayerLeaveHandler
        : IClientPacketHandler
    {
        public void Handle(NetPacketReader reader)
        {
            PlayerLeavePacket packet =
                MessagePackSerializer
                    .Deserialize<PlayerLeavePacket>(
                        reader.GetRemainingBytes());

            PlayerRegistry.Instance
                ?.Despawn(packet.NetworkId);
        }
    }
}