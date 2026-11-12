using System;
using Godot;
using Godot.Collections;
using GodotInk;
using GoldenHand.Data.Types;
using Ink.Runtime;
using Microsoft.VisualBasic;
using InkList = GodotInk.InkList;

namespace GoldenHand.Data.Models;

[GlobalClass, Tool]
public partial class CharacterComposerModel : GameStateModel
{
    [Export]
    public Dictionary<string, AbxPartyRelation> Model =  new (); 


    public override void UpdateModel(Variant data)
    {
        if (data.VariantType != Variant.Type.Dictionary)
            throw new ArgumentException(); 
        
        var model = data.As<Dictionary<string, AbxPartyRelation>>();
        Model.Merge(model);
            
    }

    public override Variant GetModel() => Model;
}
