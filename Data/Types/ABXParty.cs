using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Godot;
using Godot.Collections;
using GoldenHand.Data.Models;

namespace GoldenHand.Data.Types;

[Flags]
public enum Emotions
{
    Happiness = 1,
    Sadness   = 2,
    Surprise  = 4,
    Fear      = 8,
    Anger     = 16,
    Disgust   = 32,
    Contempt  = 64
}




[SuppressMessage("ReSharper", "InconsistentNaming")]
[GlobalClass, Tool]
public partial class ABXParty : Resource, IActor
{
    
    public string Environment { get; set; }
    public string LocalEnvironment { get; }
    public long? UID { get; set; }
    public string? Path
    {
        get => GetPath();
        set => throw new NotImplementedException();
    }

    private string name = "";
    
     
    [Export]
    public string Name
    {
        get => name;
        set
        {
            if (name == value) return;
            name = value ?? "";
            byName = null;
        }
    }

    [Export]
    public Godot.Collections.Array<ABXParty> AdjacentParties
    {
        get => adjacentParties;
        set
        {
            adjacentParties = value ?? new();
            byName = null; // invalidate index
        }
    }
    private Godot.Collections.Array<ABXParty> adjacentParties = new();

    // Lazily-built name index. Rebuilt whenever we touch the array or Name changes.
    private System.Collections.Generic.Dictionary<string, ABXParty>? byName;
    private System.Collections.Generic.Dictionary<string, ABXParty> ByName
    {
        get
        {
            if (byName != null) return byName;
            byName = new();
            foreach (var p in adjacentParties)
                if (p != null && !string.IsNullOrEmpty(p.Name))
                    byName[p.Name] = p;
            return byName;
        }
    }
    
    public ABXParty(string name)
    {
        this.name =  name;
    }

    public ABXParty()
    {
    }

    public ABXParty this[string name]
    {
        get => ByName[name];
        set
        {
            RemoveAdjacent(name);
            if (value == null) return;
            value.Name = name;
            AddAdjacent(value);
        }
    }

    public static ABXParty CreateEmptyParty(string name) => new ABXParty(name);
    public bool Contains(ABXParty? party) => party != null && adjacentParties.Contains(party);
    public bool Contains(string name)     => ByName.ContainsKey(name);
    public ABXParty? FindAdjacent(string name)
        => ByName.TryGetValue(name, out var p) ? p : null;

    public void AddAdjacent(ABXParty party)
    {
        if (party == null || party == this || string.IsNullOrEmpty(party.Name))
        { GD.PushError("AddAdjacent: invalid party"); return; }

        if (ByName.TryGetValue(party.Name, out var existing))
        {
            if (existing == party) return;
            GD.PushError($"AddAdjacent: name collision on '{party.Name}'");
            return;
        }
        adjacentParties.Add(party);
        byName![party.Name] = party;
    }

    public void RemoveAdjacent(ABXParty party) => RemoveAdjacent(party?.Name);
    public void RemoveAdjacent(string? name)
    {
        if (name == null) return;
        if (!ByName.TryGetValue(name, out var existing)) return;
        adjacentParties.Remove(existing);
        byName!.Remove(name);
    }

    public override int GetHashCode() => Name?.GetHashCode() ?? 0;
    public override bool Equals(object? obj) => obj is ABXParty p && p.Name == Name;
}


[Tool]
public partial class AbxPartyRelation : Resource
{
    
    [Export] public EmotionModel AttitudeTowardsParty { get; set; }
    [Export] public EmotionModel BelievedPartyAttitude { get; set; }
    [Export] public float Trust { get; set; }
    
}