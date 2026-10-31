using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using GodotInk;
using GoldenHand.Data.Types;

namespace GoldenHand.Data.Models;
/*
 * We will expose a story for the model to hold
 * Then we will extract the data we want into reactive types
 */

[GlobalClass]
public partial class InkStoryModel : GameStateModel
{
    [Export] private InkStory story;

    private ReactiveVariant<Array<string>> currentTags = new(); 
    private ReactiveDictionary<string, ABXContext> activeContexts = new();

    public IReadOnlyReactive<Array<string>> CurrentTags => currentTags;
    public IReadOnlyReactiveDictionary<string, ABXContext> ActiveContexts => activeContexts;
    
    public required ReactiveArray<string> CurrentLines;

    // maximal continue while updating reactive properties
    // ideally subscribers will consume the lines
    
    
    /*
     * an ink script will be between two subjects
     */
    public void ContinueStory()
    {
        while (story.CanContinue)
        { 
            CurrentLines.Add(story.Continue());
            
            if (story.CurrentTags != null)
                currentTags.Value = new(story.CurrentTags);
 
            if (!activeContexts.ContainsKey(story.CurrentFlowName))
            { 
                activeContexts.Add(story.CurrentFlowName, new ABXContext());
            }
  
        } 
    }
    
}