using Godot;
using GoldenHand.Data.Types;

namespace GoldenHand.Data.Models;


/// <summary>
/// <para><see cref="GoldenHand.Data.GameStateModel"/> for a collection of <see cref="GoldenHand.Data.Types.CardState"/></para>
/// </summary>
[GlobalClass, Tool]
public partial class CardCollectionModel : GameStateModel
{
    [Export] public ReactiveArray<CardState> Cards { get; set; } = new();

    //Dummy functions to test MVP functionality
    public CardState PopTopCard()
    {
        CardState card = Cards.Last;
        Cards.RemoveAt(Cards.Count - 1);
        return card;
    }

    public void CreateRandomDeck(int size)
    {
        Cards = new ReactiveArray<CardState>(size);
        for (int i = 0; i < Cards.Count; i++)
        {
            CardType type = (CardType)(GD.Randi() % 3);
            Cards[i] = new CardState(type);
            

        }
        
    }
}