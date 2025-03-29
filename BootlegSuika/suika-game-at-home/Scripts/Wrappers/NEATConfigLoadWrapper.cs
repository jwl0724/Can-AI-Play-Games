using Godot;
using System;

public partial class NEATConfigLoadWrapper
{
    private static readonly GDScript configLoaderScript = GD.Load<GDScript>("res://Addons/Neat/standalone_scripts/standalone_neuralnet.gd");
    private readonly GodotObject NN;
    public NEATConfigLoadWrapper(string configFile)
    {
        NN = (GodotObject) configLoaderScript.New();
        LoadConfig(configFile);
    }

    public void LoadConfig(string configName)
    {
        NN.Call("load_config", configName);
    }

    public static Godot.Collections.Array GetSavedNetworks()
    {
        return (Godot.Collections.Array) configLoaderScript.Call("get_saved_networks");
    }

    public Godot.Collections.Array<float> Update(Godot.Collections.Array input)
    {
        return (Godot.Collections.Array<float>) NN.Call("update", input);
    }
}
