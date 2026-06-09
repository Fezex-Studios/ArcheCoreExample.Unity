using MessagePack;
using UnityEngine;

namespace ArcheCore.WorldServer.PersistenceServer.Networking.P2W
{
    public class CharacterLoadHandler
        : IPersistencePacketHandler
    {
        public void Handle(Packet packet)
        {
            var character =
                MessagePackSerializer
                    .Deserialize<CharacterLoadResponse>(
                        packet.Payload);

            Debug.Log(
                $"Loaded Character");

            Debug.Log(
                $"Id={character.CharacterId}");

            Debug.Log(
                $"Name={character.Name}");

            Debug.Log(
                $"Level={character.Level}");

            Debug.Log(
                $"Pos={character.X}, {character.Y}, {character.Z}");
        }
    }
}