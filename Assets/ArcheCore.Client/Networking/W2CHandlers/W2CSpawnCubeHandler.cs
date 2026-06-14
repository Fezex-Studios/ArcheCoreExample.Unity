using LiteNetLib;
using MessagePack;
using Shared.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArcheCore.Client.Networking.W2C
{
    public class W2CSpawnCubeHandler : IClientPacketHandler
    {
        public void Handle(NetPacketReader reader)
        {
            W2CSpawnCubePacket packet =
                MessagePackSerializer
                    .Deserialize<W2CSpawnCubePacket>(
                        reader.GetRemainingBytes());

            ClientNetwork.Instance
                .StartCoroutine(SpawnCube(packet));
        }

        private System.Collections.IEnumerator SpawnCube(
            W2CSpawnCubePacket packet)
        {
            if (SceneManager.GetActiveScene().name != "main_world")
            {
                yield return new WaitUntil(() =>
                    SceneManager.GetActiveScene().name == "main_world");
            }

            GameObject cube =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.position =
                new Vector3(
                    packet.x,
                    packet.y,
                    packet.z);

            cube.name = $"Cube_{packet.CubeId}";

            Debug.Log(
                $"[SpawnCube] Spawned {cube.name} at {cube.transform.position}");
        }
    }
}