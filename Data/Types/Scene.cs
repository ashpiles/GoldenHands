using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Godot;
using Godot.Collections;


namespace GoldenHand.Data.Types;
    

 
public interface IActor
{
    public string Environment { get; set; }
    public long? UID { get; protected set; }
    public string? Path { get; protected set; } 
    
    static long GetUid(IActor actor) => actor.UID ?? throw new NullReferenceException();
    /*
     * We need to perform an operation
     * find the child in the entity tree
     * if not there we add child to the tree
     * if there is, replace the value
     */
    static bool RegisterActor(IActor parent, IActor child)
    {
        if(parent.Path == null || child.Path == null)
            return false;
        parent.UID ??= ResourceUid.CreateId();
        child.UID ??= ResourceUid.CreateId();

        string path = child.Path;
        long id = (long)child.UID;
        string environment = parent.Environment;
        
        if (!ResourceUid.HasId(id)) 
            ResourceUid.AddId(id, path);
        
        var entityEnv = EntityEnvironment<Variant>(environment);
        
        if (!entityEnv.ContainsKey(id))
            entityEnv.Add(id, ComponentEnvironment<Variant>(child));
        else
            entityEnv[id] = ComponentEnvironment<Variant>(child);
        
        string env = Json.Stringify(entityEnv);
        parent.Environment = env;
        child.Environment = env;
        
        
        
        return true;
    }

    static bool UnregisterActor(IActor actor)
    {
        // we unassign the id value
        return true;
    }

    static Godot.Collections.Dictionary<long, T> EntityEnvironment<[MustBeVariant] T>(IActor context) 
        => Json.ParseString(context.Environment).As<Godot.Collections.Dictionary<long, T>>();

    static Godot.Collections.Dictionary<long, T> EntityEnvironment<[MustBeVariant] T>(string context) 
        => Json.ParseString(context).As<Godot.Collections.Dictionary<long, T>>();
    static Godot.Collections.Dictionary<string, T> ComponentEnvironment<[MustBeVariant] T>(IActor actor)
    {
        if (actor.UID == null) throw new NullReferenceException(); 
        long id = (long)actor.UID;
        
        var entities = EntityEnvironment<Variant>(actor); 
        if (!entities.TryGetValue(id, out Variant value)) throw new KeyNotFoundException(); 
        return value.As<Godot.Collections.Dictionary<string, T>>();
        
    }
    static private bool CheckValidActor(IActor actor)
    {
        if (actor.UID == null || actor.Path == null)
        { 
            GD.PrintErr(nameof(actor)+" has not been registered");
            return false;
        }


        return true; 
    }

    static bool AssignComponent(IActor actor, Variant component)
    { 
        // unregistered
        if (actor.UID == null) return false;
        long id = (long)actor.UID;
        
        Variant env = Json.ParseString(actor.Environment);
        var environment = env.As<Godot.Collections.Dictionary<long, Variant>>();
        
        environment[id] = component;
        
        actor.Environment = Json.Stringify(environment, "\t");
        
        return true;
    } 

}


public record StorySceneEnvironment(string Environment) : IActor
{
    public string Environment { get; set; }
    public long? UID { get; set; }
    public string? Path 
    {
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    } 

    public StorySceneEnvironment(IActor actor) : this(actor.Environment) { } 
}

public record ComponentEnvironment(IActor Actor) : IActor
{
    public Godot.Collections.Dictionary<string, Variant> GetEnvironment() => IActor.ComponentEnvironment<Variant>(Actor); 
    public T GetComponent<[MustBeVariant] T>(string name) => GetEnvironment()[name].As<T>();
    
    public string Environment
    {
        get => Actor.Environment;
        set => Actor.Environment = value;
    }

    public long? UID
    {
        get => Actor.UID;
        set => throw new NotImplementedException();
    }

    public string? Path
    {
        get => Actor.Path;
        set => throw new NotImplementedException();
    }
}

// ReSharper disable once InconsistentNaming
public class StoryScene<T>
{

    // what if we are just moving along the json dom with a node?
    // the state being the current representation of the json
    // 
}





/*
 * The current scene is a big bundle of state we query
 * we can then create records as abstracted query commands
 * where binding to one of these types will execute a query.
 *
 * first thing I need is the relationship between two ABX party members
 */

