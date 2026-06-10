using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using ArcheCore.WorldServer.Lua.Scripting;
using ArcheCore.WorldServer.Lua.Scripting.Bindings;
using ArcheCore.WorldServer.Networking.W2C;
using ArcheCore.WorldServer.ServerConfig;
using LiteNetLib;
using Shared;
using Shared.Components;
using UnityEngine;

namespace ArcheCore.WorldServer.Managers
{
    public class PlayerManager
    {
        private int nextNetworkId = 1;

        private readonly Dictionary<NetPeer, int> peerToId   = new();
        private readonly Dictionary<int, int>     idToAccount = new();
        private readonly Dictionary<int, Vector3> positions   = new();

        private readonly ConcurrentQueue<Action> pendingActions = new();
        private readonly SpawnManager spawnManager;
        public IReadOnlyDictionary<NetPeer, int> PeerToId => peerToId;
        public Dictionary<int, Vector3>          Positions => positions;
        

        private readonly LuaEngine luaEngine = new();

        public void DrainActions()
        {
            while (pendingActions.TryDequeue(out Action action))
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void EnqueueAction(Action action)
        {
            pendingActions.Enqueue(action);
        }
        public PlayerManager(SpawnManager spawnManager)
        {
            this.spawnManager = spawnManager;
        }

        public void HandlePlayerConnected(NetPeer peer, int accountId)
        {
            W2CMOTDPacket.Send(peer, ConfigService.Config.MOTD);

            int newId = SpawnPlayer(peer, accountId);

            LuaPlayer luaPlayer = new LuaPlayer(peer, newId, accountId);

            string scriptPath = Path.Combine(
                Application.streamingAssetsPath,
                "Lua",
                "Server",
                "on_player_connect.lua");

            luaEngine.RunFile(scriptPath);
            luaEngine.CallFunction("on_player_connect", luaPlayer);
            
            Debug.Log($"spawnManager null? {spawnManager == null}");            
            spawnManager.SendCubesToPeer(peer);
            // Send all existing players to the new joiner
            foreach (var kvp in peerToId)
            {
                int existingId = kvp.Value;

                if (existingId == newId)
                    continue;

                PacketSender.SendPacket(
                    peer,
                    PacketType.SpawnPlayer,
                    new SpawnPlayerPacket
                    {
                        NetworkId    = existingId,
                        x            = positions[existingId].x,
                        y            = positions[existingId].y,
                        z            = positions[existingId].z,
                        IsLocalPlayer = false
                    });
            }
        }

        public void HandlePlayerDisconnected(NetPeer peer)
        {
            if (!peerToId.TryGetValue(peer, out int networkId))
                return;

            peerToId.Remove(peer);
            idToAccount.Remove(networkId);
            positions.Remove(networkId);

            WorldLogger.Info($"Player {networkId} disconnected");

            // Tell every remaining client to despawn this player
            foreach (var kvp in peerToId)
            {
                PacketSender.SendPacket(
                    kvp.Key,
                    PacketType.PlayerLeave,
                    new PlayerLeavePacket
                    {
                        NetworkId = networkId
                    });
            }
        }

        public void BroadcastPosition(
            NetPeer sender,
            int     networkId,
            Vector3 position)
        {
            foreach (var kvp in peerToId)
            {
                NetPeer peer = kvp.Key;

                if (peer == sender)
                    continue;

                PacketSender.SendPacket(
                    peer,
                    PacketType.PlayerPosition,
                    new PlayerPositionPacket
                    {
                        NetworkId = networkId,
                        x         = position.x,
                        y         = position.y,
                        z         = position.z
                    },
                    DeliveryMethod.Unreliable);
            }
        }

        private int SpawnPlayer(NetPeer peer, int accountId)
        {
            int     networkId     = nextNetworkId++;
            Vector3 spawnPosition = new Vector3(0, 1, 0);

            peerToId[peer]          = networkId;
            idToAccount[networkId]  = accountId;
            positions[networkId]    = spawnPosition;

            foreach (var kvp in peerToId)
            {
                bool isLocal = kvp.Key == peer;

                PacketSender.SendPacket(
                    kvp.Key,
                    PacketType.SpawnPlayer,
                    new SpawnPlayerPacket
                    {
                        NetworkId    = networkId,
                        x            = spawnPosition.x,
                        y            = spawnPosition.y,
                        z            = spawnPosition.z,
                        IsLocalPlayer = isLocal
                    });
            }

            WorldLogger.Info($"Spawned player {networkId} (AccountId={accountId})");

            return networkId;
        }
    }
}