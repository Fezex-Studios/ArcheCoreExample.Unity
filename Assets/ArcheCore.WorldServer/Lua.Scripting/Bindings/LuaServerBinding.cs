// Server/Scripting/Bindings/LuaServerBinding.cs
using MoonSharp.Interpreter;
using UnityEngine;

namespace ArcheCore.WorldServer.Lua.Scripting.Bindings
{
    [MoonSharpUserData]
    public class LuaServerBinding
    {
        public void Log(string message)
        {
            Debug.Log($"[Lua] {message}");
        }

        public string GetTime()
        {
            return System.DateTime.Now
                .ToString("HH:mm:ss");
        }
    }
}