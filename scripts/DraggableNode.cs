using Godot;

namespace GoldenHand.scripts;


[GlobalClass]
public partial class DraggableNode : BaseButton
{
    [Export] public Control Model;
    public override void _Ready()
    {
        Model.OffsetTransformEnabled = true;
        ButtonDown += UpdatePosition;
        ButtonUp += ClearPosition;
        GD.Print("test");
    }

    public override void _Pressed()
    {
        GD.Print("Pressed");
    }

    public void UpdatePosition()
    {
        Model.OffsetTransformPosition = GetGlobalMousePosition() - GetGlobalPosition();
        GD.Print("position offset to set: " + OffsetTransformPosition);
    }

    public void ClearPosition()
    {
        Model.OffsetTransformPosition = Vector2.Zero;
    }
    
}