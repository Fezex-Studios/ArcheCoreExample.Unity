using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ArcheCore.WorldServer.PersistenceServer.Networking;
using ArcheCore.WorldServer.PersistenceServer.Networking.P2W;
using ArcheCore.WorldServer.PersistenceServer.Packets;
using ArcheCore.WorldServer.ServerConfig;
using MessagePack;

using UnityEngine;

namespace Worldserver.ArcheCore.PersistenceServer.Scripts
{
    public class PersistenceClient
        : MonoBehaviour
    {
        private TcpClient client;
        private NetworkStream stream;

        private PersistenceDispatcher
            dispatcher;

        async void Start()
        {
            dispatcher =
                new PersistenceDispatcher();

            RegisterHandlers();

            client =
                new TcpClient();

            await client.ConnectAsync(
                "127.0.0.1",
                ConfigService.Config.PersistencePort);

            stream =
                client.GetStream();
            
            _ = ReceiveLoop();

            await Send(
                PersistenceOpcode.W2PConnectRequest,
                new W2PConnectionRequest
                {
                    Message = "WorldServer 1 has connected"
                });
        }

        private void RegisterHandlers()
        {
            dispatcher.Register(
                PersistenceOpcode.P2WConnectResponse,
                new P2WConnectResponseHandler());

            dispatcher.Register(
                PersistenceOpcode.CharacterLoad,
                new CharacterLoadHandler());
        }

        public async Task Send<T>(
            PersistenceOpcode opcode,
            T payload)
        {
            Packet packet =
                new Packet
                {
                    Opcode =
                        (ushort)opcode,

                    Payload =
                        MessagePackSerializer
                            .Serialize(payload)
                };

            byte[] packetBytes =
                MessagePackSerializer
                    .Serialize(packet);

            byte[] lengthBytes =
                BitConverter.GetBytes(
                    packetBytes.Length);

            await stream.WriteAsync(
                lengthBytes);

            await stream.WriteAsync(
                packetBytes);

            Debug.Log(
                $"[World] Sent {opcode}");
        }

        private async Task ReceiveLoop()
        {
            while (client.Connected)
            {
                try
                {
                    byte[] lengthBuffer =
                        new byte[4];

                    int read =
                        await ReadExact(
                            lengthBuffer,
                            4);

                    if (read == 0)
                        break;

                    int packetLength =
                        BitConverter.ToInt32(
                            lengthBuffer,
                            0);

                    byte[] packetBuffer =
                        new byte[packetLength];

                    await ReadExact(
                        packetBuffer,
                        packetLength);

                    Packet packet =
                        MessagePackSerializer
                            .Deserialize<Packet>(
                                packetBuffer);
                    
                    Debug.Log($"[World] Received {(PersistenceOpcode)packet.Opcode}");

                    dispatcher.Handle(
                        packet);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }

        private async Task<int> ReadExact(
            byte[] buffer,
            int size)
        {
            int totalRead = 0;

            while (totalRead < size)
            {
                int read =
                    await stream.ReadAsync(
                        buffer,
                        totalRead,
                        size - totalRead);

                if (read == 0)
                    return 0;

                totalRead += read;
            }

            return totalRead;
        }

        private void OnDestroy()
        {
            stream?.Close();
            client?.Close();
        }
    }
}