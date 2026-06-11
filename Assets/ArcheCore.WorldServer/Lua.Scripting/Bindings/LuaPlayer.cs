// Server/Scripting/Bindings/LuaPlayer.cs

using ArcheCore.WorldServer.Networking.W2C;
using LiteNetLib;

using MoonSharp.Interpreter;

namespace ArcheCore.WorldServer.Lua.Scripting.Bindings
{
    [MoonSharpUserData]
    public class LuaPlayer
    {
        private readonly NetPeer peer;
        public int NetworkId { get; }
        public int AccountId { get; }

        public LuaPlayer(
            NetPeer peer,
            int networkId,
            int accountId)
        {
            this.peer = peer;
            NetworkId = networkId;
            AccountId = accountId;
        }

        // Methods Lua can call
        public void SendAnnouncementMessage(string message)
        {
            W2CAnnouncementPacketSender.Send(peer, message);
        }

        public void Kick(string reason)
        {
            peer.Disconnect();
        }
    }
}