using Godot;
using GoldenHand.Data.Types;

namespace GoldenHand.Data.Flyweights;


/// <summary>
/// <para><see cref="Godot.Resource"/> flyweight to translate <see cref="GoldenHand.Data.Types.CardState"/> to a <see cref="Godot.Texture2D"/></para>
/// </summary>
[GlobalClass]
public partial class CardFlyweight : Resource
{
    
    [Export] public CardType Type { get; set; } =  CardType.Lie;
    [Export] public Texture2D CardArt { get; set; } 
    
    //public CardState ToRecord(bool isFaceUp = true) => new(Type, isFaceUp); 
}