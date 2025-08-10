using System;
using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;

public class ScriptManager
{
    // Cache dictionary: script file path -> compiled Script instance
    private Dictionary<string, Script> _scriptCache = new Dictionary<string, Script>();

    /// <summary>
    /// Loads a script from path or returns cached if already loaded.
    /// </summary>
    public Script GetScript(string path)
    {
        if (_scriptCache.TryGetValue(path, out var cachedScript))
        {
            return cachedScript;
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Lua script not found: {path}");

        }
            

        string code = File.ReadAllText(path);
        var script = new Script();

        // Inject any C# functions your Lua scripts need
        script.Globals["printMessage"] = (Action<string>)(msg => Console.WriteLine(msg));

        // Run the script once to initialize globals, tables, functions
        script.DoString(code);
        

        _scriptCache[path] = script;
        return script;
    }
}
