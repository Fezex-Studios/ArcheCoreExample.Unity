using System.Threading.Tasks;
using ArcheCore.WorldServer.Managers;
using LiteNetLib;
using MessagePack;
using MMO.Shared.Packets;
using Shared.AuthService;
using UnityEngine;

namespace ArcheCore.WorldServer.Networking.C2W
{
    public class C2WAuthenticateHandler : IPacketHandler
    {
        private readonly PlayerManager playerManager;

        public C2WAuthenticateHandler(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public void Handle(
            NetPeer peer,
            NetPacketReader reader)
        {
            AuthenticateRequest request =
                MessagePackSerializer
                    .Deserialize<AuthenticateRequest>(
                        reader.GetRemainingBytes());

            Debug.Log($"Auth Token: {request.Token}");

            _ = ValidateAndConnect(peer, request.Token);
        }

        private async Task ValidateAndConnect(NetPeer peer, string token)
        {
            int accountId = await AuthService.ValidateToken(token);

            if (accountId == -1)
            {
                Debug.LogWarning(
                    $"[C2WAuthenticateHandler] Invalid or expired token — disconnecting peer {peer.Address}");

                playerManager.EnqueueAction(() =>
                    peer.Disconnect());

                return;
            }

            Debug.Log(
                $"[C2WAuthenticateHandler] Token valid. AccountId={accountId} — spawning player.");

            playerManager.EnqueueAction(() =>
                playerManager.HandlePlayerConnected(peer, accountId));
        }
    }
}