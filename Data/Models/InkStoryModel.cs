using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Godot;
using Godot.Collections;
using GodotInk;
using GoldenHand.Data.Types;

namespace GoldenHand.Data.Models;
/*
 * We will expose a story for the model to hold
 * Then we will extract the data we want into reactive types
 */

[GlobalClass, Tool]
public partial class InkStoryModel : GameStateModel
{

    [Export] public required InkStory Story; 
    [Export] private Godot.Collections.Dictionary<ABXParty, CharacterComposerModel> composers = new();
    
    public required ReactiveArray<string> CurrentLines;

    private StoryScene<StorySceneEnvironment, IActor> mainStoryScene = new((env) => env as IActor);



     
    public void ContinueStory()
    {
        while (Story.CanContinue)
        {
            Story.Continue();
            
            /*
             * The scene
             */
            
            mainStoryScene.Bind<ABXParty>((sceneElement) =>
            {
                // we registered the parent and child 
                return new(scene =>
                {
                    if (sceneElement is not ABXParty party)
                        throw new ArgumentException($"Cannot transform {nameof(sceneElement)} to {nameof(ABXParty)}");

                    if (IActor.RegisterActor(scene, sceneElement))
                        return sceneElement as ABXParty ??
                               throw new ArgumentNullException($"{nameof(sceneElement)} is null");

                    throw new ArgumentException(
                        $"Cannot register {nameof(sceneElement)} to {nameof(scene)} while binding");
                }); 
            }).Bind<ComponentEnvironment>(party =>
            {
                return new StoryScene<StorySceneEnvironment, ComponentEnvironment>(env =>
                {
                    /*
                     * [] read prev emotion states from env
                     * [] write new emotion states w/ party
                     *
                     * this process needs a bit more ironing out
                     *
                     * lets read a previous state and bind a new state driven by ink
                     * this should update our model
                     *
                     * then read prev state and bind a new one driven by game code
                     * this should also update our model
                     *
                     * we use the monad to correctly navigate the hierarchy for an operation
                     *
                     * So a scene should be converting IActor types into GameStateModels?
                     * our environment is the IActor and the model is its concrete representation
                     */
                    var entities = IActor.EntityEnvironment<Variant>(env);
                    var sceneComponents = IActor.ComponentEnvironment<Variant>(env);
                    
                    if (!entities.ContainsKey(IActor.GetUid(party)))
                        IActor.RegisterActor(env, party); // this should work w/ componenets too mebe

                    
                    if (!composers.ContainsKey(party)) throw new KeyNotFoundException($"{nameof(composers)} not found");
                    if (!sceneComponents.ContainsKey("actors")) throw new KeyNotFoundException($"Key 'actors' not found in {nameof(sceneComponents)}");
                    Variant variantActors = sceneComponents["actors"];
                    Array<string> actors = variantActors.VariantType == Variant.Type.Array ? variantActors.As<Array<string>>() : throw new ArgumentException("Variant type different than expected");
                    var comp =  composers[party];

                    foreach (var actor in actors)
                    {
                    }
                    
                    
                    // update relations from environment
                    // connect to the composer model which hosts the truth
                    // we update those states from what we read with the party

                    // source
                    //target
                    
                    return (env);
                }); 
            });
            
            
            // state is just a dictionary of flags and args
            
            // gather state from the story
                // we gather this as flag arg pairs
                // flags are composed by the type inheriting IActor
                
                // example:
                /* We need to get the relationship between two parties
                 *
                 * Inside the current state we have:
                 *  -the root party we search from
                 *  -the other parties we check adjacency too
                 * from this we can then traverse our state for the relation between the two
                 * Since their history has been slowly building in the growing mountain of state
                 *
                 * relation data tables in memory makes the most sense
                 * which will be the composers
                 *
                 * we will read the current actors in the scene and load the env w/ the relevant states
                 * from the relations
                 * 
                 */
                
                // bind function which encodes the relation into the environment
                // this is a collection of states shared between ink and the code
                // through moving in the story we update our state between the two and coordinate
                // how we move through the story based on this collection of states
                // You can think of it like Ink Script is the path with the states as the driver
                // There is a graph of connections with truly unique edges, as each movement along an edge
                // transforms the state.
                
                  
        }
       
        
    }

    private void ApplyEmotionWeights(Emotions emotions, float weight)
    {
        
    }

    public override void UpdateModel(Variant data)
    {
        throw new NotImplementedException();
    }

    public override Variant GetModel()
    {
        throw new NotImplementedException();
    }
}