namespace Shared
{
    public enum PacketType : byte
    {
        Connect      = 1,
        SpawnPlayer  = 2,
        MOTD         = 3,
        PlayerMove   = 4,
        PlayerPosition = 5,
        Authenticate = 6,
        PlayerLeave  = 7,
    }
}