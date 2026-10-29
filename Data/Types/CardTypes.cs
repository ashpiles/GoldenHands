using Godot;

namespace GoldenHand.Data.Types;

public enum CardType
{
    Lie,
    Dismiss,
    Relate 
}

/// <summary>
/// <para>Type denoting the current state of a Card</para>
/// </summary>
public partial class CardState : RefCounted
{
    public CardType Type;
    public bool FaceUp;
    
    public CardState() { }
    public CardState(CardType type, bool isFaceUp = false)
    {
        Type = type;
        FaceUp = isFaceUp;
    }

}
