using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Godot;
using Godot.Collections;

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
public partial class ABXContext : Resource
{
    [Export] public required string Name;
    [Export] public required Array<ABXParty> Parties;

}   


[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class ABXParty : Resource
{
    [Export] public required string Name;
    [Export] public required ABXParty Other;
    [Export] public required ABXContext Context;

} 

