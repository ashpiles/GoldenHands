using System;
using System.Collections.Generic;
using Godot;

namespace GoldenHand.Data;


/// <summary>
/// <para><see cref="Godot.Resource"/> that holds relevant game data</para>
/// <para>Allows stateful data to be decoupled from an owning object</para>
/// <para>Intended to be used with <see cref="GoldenHand.Data.Reactive"/> member variables</para>
/// </summary>
[Tool]
public abstract partial class GameStateModel : Resource
{
    
    /*
     * game state models can convert their data into a dictionary of string, Variant type
     */

    public abstract void UpdateModel(Variant data);
    public abstract Variant GetModel();
}