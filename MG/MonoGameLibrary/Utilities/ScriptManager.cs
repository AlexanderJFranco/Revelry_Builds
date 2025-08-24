using System;
using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;
using MonoGameLibrary.Dialogue;

namespace MonoGameLibrary.Utilities;

public class ScriptManager
{

    private Dictionary<string, Script> _scriptCache;
    // Cache dictionary: script file path -> dialogue table
    private readonly Dictionary<string, Table> _dialogueCache = new();
    public ScriptManager()
    {
        _scriptCache = new Dictionary<string, Script>();
        
    }
    // Cache dictionary: script file path -> compiled Script instance


    /// <summary>
    /// Loads a script from path or returns cached if already loaded.
    /// </summary>
    public Script GetScript(string folder, string file)
    {
        //Build script directory path
        string path = Path.Combine(folder, file);
        //If script is currently contained within Cache, return cache
        if (_scriptCache.TryGetValue(file, out var cachedScript))
        {
            return cachedScript;
        }

        //If file is missing return error
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Lua script not found: {path}");

        }


        //Store entirety of Lua script within 'code' string
        string code = File.ReadAllText(path);
        var script = new Script();

        // Inject any C# functions your Lua scripts need
        script.Globals["printMessage"] = (Action<string>)(msg => Console.WriteLine(msg));

        // Run the script once to initialize globals, tables, functions
        script.DoString(code);

        // Grab dialogue table
        var dialogueTable = script.Globals.Get("dialogue");
        if (dialogueTable.Type != DataType.Table)
            throw new Exception($"Lua script {file} does not define a 'dialogue' table.");


        
        //Store script within Cache
        _scriptCache[file] = script;
        _dialogueCache[file] = dialogueTable.Table;
        
        return script;
    }
    
   
}
