using Godot;
using GoldenHand.data;

namespace GoldenHand.scripts;

public partial class Card : Control
{
	
	[Export] public CardFlyWeight Data;
	[Export] protected Texture2D CardBack;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
