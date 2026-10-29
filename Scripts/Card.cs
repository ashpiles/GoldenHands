using Godot;
using GoldenHand.Data;
using GoldenHand.Data.Flyweights;

namespace GoldenHand.Scripts;

public partial class Card : Control
{
	
	[Export] public CardFlyweight Data;
	[Export] protected Texture2D CardBack;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
