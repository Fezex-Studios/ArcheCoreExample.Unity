using System;

namespace ArcheCore.WorldServer.ServerConfig
{
    [Serializable]
    public class ServerConfig
    {
        public string ServerName = "Arc MMO";

        public int Port = 7777;

        public int MaxPlayers = 1000;

        public string PersistenceHost = "127.0.0.1";

        public int PersistencePort = 7778;

        public string MOTD = "HELLO WELCOME TO THE SERVER!!!";

        // URL of your Node.js auth server
        public string AuthServerUrl = "http://127.0.0.1:3000";
    }
}