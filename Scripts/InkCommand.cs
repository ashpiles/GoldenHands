using Godot;

namespace GoldenHand.scripts;


public interface IInkCommand
{
    void Execute(string[] args);
    void Undo();

}

public class DealCard : IInkCommand
{
    public void Execute(string[] args)
    {
        
    }
    
    public void Undo()
    {}
}

public class StartScene : IInkCommand
{
    private const string prefix = "res://levels/";
    private const string suffix = ".tscn";
    
    public void Execute(string[] args)
    {
        if (args.Length <= 0)
            return;
        
        lastScene = currentScene;
        currentScene = ResourceLoader.Load<PackedScene>(prefix + args[0] + suffix);

        SwitchScene(currentScene);
    }

    public void Undo()
    {
        var temp = lastScene;
        if (!SwitchScene(lastScene)) 
            GD.PrintErr("could not switch to last scene: " + lastScene.GetName()); 
        lastScene = currentScene;
        currentScene = temp;

    }


    private bool SwitchScene(PackedScene scene)
    {
        if (scene == null)
        {
            GD.PrintErr("Failed to switch scenes: given scene is null");
            return false;
        }
        if (Engine.GetMainLoop() is not SceneTree tree)
        {
            GD.PrintErr("Failed to switch scenes: Main Loop is not scene tree");
            return false;
        }
        
        tree.Root.AddChild(scene.Instantiate()); 
        return true;
    }

    private PackedScene currentScene;
    private PackedScene lastScene;
    
}