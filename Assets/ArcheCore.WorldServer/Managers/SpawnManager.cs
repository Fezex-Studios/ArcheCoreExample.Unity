using System.Collections.Generic;
using ArcheCore.WorldServer.Networking.W2C;
using LiteNetLib;
using UnityEngine;

namespace ArcheCore.WorldServer.Managers
{
    public class SpawnManager
    {
        private int nextCubeId = 1;

        // All active cubes: id -> position
        private readonly Dictionary<int, Vector3> cubes = new();

        // Call this once on world server Start()
        public void SpawnInitialCubes()
        {
            SpawnCube(new Vector3( 3f, 0.5f,  0f));
            SpawnCube(new Vector3(-3f, 0.5f,  0f));
            SpawnCube(new Vector3( 0f, 0.5f,  4f));
            SpawnCube(new Vector3( 0f, 0.5f, -4f));
            SpawnCube(new Vector3( 5f, 0.5f,  5f));

            WorldLogger.Info($"[SpawnManager] Spawned {cubes.Count} cubes.");
        }

        private void SpawnCube(Vector3 position)
        {
            int id = nextCubeId++;
            cubes[id] = position;
        }

        // Send all cubes to a single peer (call on player connect)
        public void SendCubesToPeer(NetPeer peer)
        {
            foreach (var kvp in cubes)
            {
                W2CSpawnCubePacketSender.Send(
                    peer,
                    kvp.Key,
                    kvp.Value.x,
                    kvp.Value.y,
                    kvp.Value.z);
            }
        }
    }
}