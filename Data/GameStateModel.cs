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
public partial class GameStateModel : Resource
{
}