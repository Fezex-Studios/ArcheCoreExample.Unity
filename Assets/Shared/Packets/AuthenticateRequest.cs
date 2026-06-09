using MessagePack;

namespace MMO.Shared.Packets
{
    [MessagePackObject(true)]
    public class AuthenticateRequest
    {
        public string Token;
    }
}