-- Assets/Lua/Server/on_player_connect.lua

function on_player_connect(player)
    Server.Log("Player connected: " .. player.NetworkId)
    player.SendAnnouncementMessage("Welcome This is an Announcement")
end