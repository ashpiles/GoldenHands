using Godot;
using GoldenHand.Data;
using GoldenHand.Data.Models;
using GoldenHand.Scripts.UserInterface.Views;

namespace GoldenHand.Scripts.UserInterface.Presenters;

[GlobalClass, Tool]
public partial class DialoguePresenter : Presenter<DialogueView, InkStoryModel>
{

    [Export] private string characterName = "";
    public override void _Ready()
    {
        base._Ready();
        Model.CurrentLines.OnCreated += LinesAdded;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Model.CurrentLines.OnCreated -= LinesAdded;
    }

    private void LinesAdded(Variant lines)
    {
        /*
         * check if its for our character
         * if so consume the line
         * we need the line and the tags that come with it
         * this will help us apply emotion value changes
         *
         *
         * Emotion values will decide on word choice and sentences, which is handled by ink
         * I can then represent these values inside ink with Lists and values
         * each character has a script and if both characters have access to a knot of the same name they can have a conversation
         * So then we need two kinds of ink scripts for a character. It's topic choice script, and its dialogue script.
         */
    }
}