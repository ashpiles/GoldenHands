using System;
using System.Linq;
using Godot;
using Godot.Collections;
using GoldenHand.Data;
using GoldenHand.Scripts.UserInterface.Views;

namespace GoldenHand.Scripts.UserInterface.Presenters;

/// <summary>
/// <para><see cref="Godot.Control"/> node which bridges a <see cref="GoldenHand.Data.GameStateModel"/> and a <see cref="GoldenHand.Scripts.UserInterface.Views.View"/></para>
/// </summary>
/// <typeparam name="TView">The <see cref="GoldenHand.Scripts.UserInterface.Views.View"/> type that defines user actions and graphics</typeparam>
/// <typeparam name="TModel">The <see cref="GoldenHand.Data.GameStateModel"/> type we retrieve data from and operate on</typeparam>
[Tool]
public abstract partial class Presenter<TView, TModel> : Control
    where TView : View 
    where TModel : GameStateModel
{

    [Export] public GameStateModel ModelResource;
    protected TView? View => GetChildren().OfType<TView>().Select(view => (TView)view).FirstOrDefault();
    protected TModel? Model => (TModel)ModelResource;

    public override void _Ready()
    {
        ArgumentNullException.ThrowIfNull(View, $"{GetType().Name} requires a {typeof(TView).Name} child node.");
        ArgumentNullException.ThrowIfNull(Model, $"{GetType().Name} requires a {typeof(TModel).Name} resource.");
    }
    public override string[] _GetConfigurationWarnings()
    {
        Array<string> warnings = new();
        
        if (View is null)
        {
            warnings.Add(
                $"{GetType().Name} requires a {typeof(TView).Name} child node."
                );
        }

        if (Model is null || Model is not TModel)
        {
            warnings.Add(
                $"{GetType().Name} requires a {typeof(TModel).Name} resource."
                );
        }
        
        return warnings.ToArray();
    }
}