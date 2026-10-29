using Godot;

namespace GoldenHand.Scripts.UserInterface.Views;
 
/// <summary>
/// <see cref="Godot.Control"/> node which exposes relevant events for the <see cref="GoldenHand.Scripts.UserInterface.Presenters.Presenter"/>
/// </summary>
public abstract partial class View : Control
{
    // A dummy function
    /*
     * this implementation of the MVP pattern resulted in some empty base classes so this
     * is a function prodding the waters for any specific functionality a "view" can have
     *
     * it's still a bit hazy what exactly the project demands of a view type but this is serviceable for now
     * the pattern needs to be used for more things for it to be clearer
     */
     
    public abstract void UpdateView();
    

}