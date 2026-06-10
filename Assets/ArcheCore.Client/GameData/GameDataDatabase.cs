using System.IO;
using SQLite;
using UnityEngine;

namespace ArcheCore.Client.GameData
{
    /// <summary>
    /// Manages the single read-only connection to gamedata.db.
    /// Call Initialize() once during bootstrap before any repository is used.
    /// </summary>
    public static class GameDataDatabase
    {
        public static SQLiteConnection Connection { get; private set; }

        public static bool IsReady { get; private set; }

        public static void Initialize()
        {
            if (IsReady)
                return;

            string dbPath = Path.Combine(
                Application.streamingAssetsPath,
                "GameData",
                "gamedata.db");

            if (!File.Exists(dbPath))
            {
                Debug.LogError(
                    $"[GameDataDatabase] gamedata.db not found at: {dbPath}");
                return;
            }

            SQLiteConnectionString options = new SQLiteConnectionString(
                dbPath,
                SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex,
                storeDateTimeAsTicks: false);

            Connection = new SQLiteConnection(options);
            IsReady    = true;

            Debug.Log("[GameDataDatabase] Opened gamedata.db (read-only)");
        }

        public static void Close()
        {
            if (Connection == null)
                return;

            Connection.Close();
            Connection = null;
            IsReady    = false;

            Debug.Log("[GameDataDatabase] Closed gamedata.db");
        }
    }
}