using Godot;

namespace GoldenHand.data;

public enum CardType
{
    Disturb,
    Coax,
    Mirror
}


public record CardStateRecord(CardType Type, bool FaceUp);

[GlobalClass]
public partial class CardFlyWeight : Resource
{
    
    [Export] public CardType Type { get; set; } =  CardType.Disturb;
    [Export] public Texture2D CardArt { get; set; } 
    
    public CardStateRecord ToRecord(bool isFaceUp = true) => new(Type, isFaceUp); 

}