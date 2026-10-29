using Godot;
using GoldenHand.Data.Models;
using GoldenHand.Scripts.UserInterface.Views;

namespace GoldenHand.Scripts.UserInterface.Presenters;

/// <summary>
/// <para><see cref="GoldenHand.Scripts.UserInterface.Presenters.Presenter"/> for a <see cref="GoldenHand.Scripts.UserInterface.Views.DragDropView"/> and a <see cref="GoldenHand.Data.Models.CardCollectionModel"/></para>
/// </summary>
[GlobalClass, Tool]
public partial class DeckPresenter : Presenter<DragDropView, CardCollectionModel>
{
	public override void _Ready()
	{
		base._Ready();
		View.MouseEntered += OnMouseEnter;
		View.MouseExited += OnMouseExit;
		View.GuiInput += HandleGuiInput;
		Model.CreateRandomDeck(20);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		View.MouseEntered -= OnMouseEnter;
		View.MouseExited -= OnMouseExit;
		View.GuiInput -= HandleGuiInput;
	}

	private void OnMouseEnter()
	{}
	
	private void OnMouseExit()
	{}

	/*
	 * need to hook into godot inputs a little better here
	 */
	private void HandleGuiInput(InputEvent input)
	{
		if (input is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.Pressed && eventMouseButton.GetButtonIndex() == MouseButton.Left)
			{
				GD.Print(Model.PopTopCard().Type.ToString());
				
			}
		}
	}
}
