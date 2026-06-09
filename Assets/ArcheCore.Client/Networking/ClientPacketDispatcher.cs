using System.Collections.Generic;
using LiteNetLib;

using Shared;

namespace ArcheCore.Client.Networking
{
    public class PacketDispatcher
    {
        private readonly Dictionary<
                PacketType,
                IClientPacketHandler>
            handlers = new();

        public void Register(
            PacketType packet,
            IClientPacketHandler handler)
        {
            handlers[packet] =
                handler;
        }

        public void Handle(
            PacketType packet,
            NetPacketReader reader)
        {
            if(handlers.TryGetValue(
                   packet,
                   out var handler))
            {
                handler.Handle(
                    reader);
            }
        }
    }
}