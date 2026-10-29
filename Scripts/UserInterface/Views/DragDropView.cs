using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GoldenHand.Data;
using GoldenHand.Data.Models;

namespace GoldenHand.Scripts.UserInterface.Views;



/// <summary>
/// <para><see cref="GoldenHand.Scripts.UserInterface.Views.View"/> which facilitates drag and drop functionality</para>
/// <para>Adds and removes nodes from a <see cref="Godot.Container"/> according to the parent <see cref="GoldenHand.Scripts.UserInterface.Presenters.Presenter"/></para>
/// </summary>
[GlobalClass, Tool]
public partial class DragDropView : View
{

    [Signal] public delegate void DropEventHandler();
    [Signal] public delegate void DragEventHandler();
    private Container? SocketContainer => GetChildren().OfType<Container>().Select(child => child).FirstOrDefault();
    private HashSet<Node> containerChildren = new();
    public override void _Ready()
    {
        ArgumentNullException.ThrowIfNull(SocketContainer,
            $"{nameof(DragDropView)} requires a Container child Node.");

        if (SocketContainer is null)
            return;
        foreach (var child in SocketContainer.GetChildren())
        {
            if (child is Node node)
                containerChildren.Add(node);
        }
        
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (SocketContainer is null)
        {
            return new[]
            {
                $"{nameof(DragDropView)} requires a Container child Node."
            };
        }
        return Array.Empty<string>();
    }

    public override void UpdateView()
    {
    }



}