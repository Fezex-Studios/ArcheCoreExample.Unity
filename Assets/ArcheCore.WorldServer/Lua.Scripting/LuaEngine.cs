using System.IO;
using ArcheCore.WorldServer.Lua.Scripting.Bindings;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

namespace ArcheCore.WorldServer.Lua.Scripting
{
    public class LuaEngine
    {
        private readonly Script script;

        public LuaEngine()
        {
            script = new Script();

            // Override Unity's default loader with a filesystem loader
            script.Options.ScriptLoader = new FileSystemScriptLoader
            {
                ModulePaths = new string[] { "?", "?.lua" }
            };

            RegisterBindings();
        }

        private void RegisterBindings()
        {
            UserData.RegisterType<LuaServerBinding>();
            UserData.RegisterType<LuaPlayer>();

            script.Globals["Server"] = UserData.Create(new LuaServerBinding());
        }

        public void RunFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning(
                    $"[LuaEngine] Script not found: {path}");
                return;
            }

            try
            {
                script.DoFile(path);
            }
            catch (ScriptRuntimeException e)
            {
                Debug.LogError(
                    $"[LuaEngine] Runtime error in {path}: {e.DecoratedMessage}");
            }
        }

        public void CallFunction(
            string functionName,
            params object[] args)
        {
            DynValue fn =
                script.Globals.Get(functionName);

            if (fn.Type != DataType.Function)
            {
                Debug.LogWarning(
                    $"[LuaEngine] Function not found: {functionName}");
                return;
            }

            try
            {
                script.Call(fn, args);
            }
            catch (ScriptRuntimeException e)
            {
                Debug.LogError(
                    $"[LuaEngine] Error calling {functionName}: {e.DecoratedMessage}");
            }
        }
    }
}