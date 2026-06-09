using MessagePack;
using UnityEngine;

namespace ArcheCore.WorldServer.PersistenceServer.Networking.P2W
{
    public class HelloHandler
        : IPersistencePacketHandler
    {
        public void Handle(Packet packet)
        {
            var hello =
                MessagePackSerializer
                    .Deserialize<HelloPayload>(
                        packet.Payload);

            Debug.Log(
                $"Received: {hello.Message}");
        }
    }
}