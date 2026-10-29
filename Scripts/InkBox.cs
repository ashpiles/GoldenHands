using System;
using Godot;
using Godot.Collections;
using GodotInk;
using GoldenHand.scripts;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;

public partial class InkBox : VBoxContainer
{
	[Export]
	private InkStory story;

	private System.Collections.Generic.Dictionary<string, IInkCommand> tagCommands = new()
	{
		{"start_scene", new StartScene()},
	};

	public void WipeInkContent(int amount = 0)
	{
		Array<Node> children = new  Array<Node>();
		amount = amount > 0 ? amount : GetChildCount();
		for (int i = 0; i < amount; i++)
		{
			var child = GetChild(i);
			children.Add(child);
			RemoveChild(child); 
			child.QueueFree();
		} 
	}
	public override void _Ready()
	{ 
	}

	public void HandleTag(string tag)
	{
		var (command, args) = tag.Split(':', 2) is [var c, var a] ? (c, a) : throw new FormatException();
		tagCommands[command].Execute(args.Split(','));
		
		/* dumb command decorations */
		if (command is "start_scene") WipeInkContent();
	}

	private void ContinueStory()
	{
		foreach (Node child in GetChildren())
			child.QueueFree();

		RichTextLabel content = new() { Text = story.Continue() };
		//content.AutowrapMode = TextServer.AutowrapMode.WordSmart;
		
		AddChild(content);

		foreach (var tag in story.CurrentTags)
			HandleTag(tag);

		foreach (InkChoice choice in story.CurrentChoices)
		{
			Button button = new() { Text = choice.Text };
			button.Pressed += delegate
			{
				story.ChooseChoiceIndex(choice.Index);
				ContinueStory();
			};
			AddChild(button);
		}
	}
}

/*
 * This must fit in two different kinds of dialogue boxes
 * It will move us to different scenes and basically moves the whole story along
 *
 * We need to parse the inkscript to move the game forwards
 * Switch scenes X
 * Set game states
 *
 * The cards you play should be interpreted as the dialogue choices you make
 * And each choice should come with a confidence wager, some wagers even banking more
 * No the beginning section is not necessary
 * cut the beginning, have it all exist as a card game
 *
 * We start with a black screen asking some questions and you are dealt the cards
 * you then answer the questions with said cards and that is the tutorial
 * How you answer the tutorial will affect your deck in the real game
 *
 * The tutorial will have you begin with just playing a card prompted by a question
 * then you will be posed with another question in response to that, you will have one each type of card and it will show the
 * defense card you have to beat.
 */
