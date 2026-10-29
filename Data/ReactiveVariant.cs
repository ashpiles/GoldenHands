using System;
using System.Collections.Specialized;
using Godot;

namespace GoldenHand.Data;

/// <summary>
/// <para>The base class of the reactive type</para>
///
/// <para>Reactive variables emit a <see cref="GoldenHand.Data.Reactive.ReactiveChanged"/> signal when a change has been made </para>
/// <para>Child classes are responsible for the logic behind emitting these signals</para>
/// </summary>
public abstract partial class Reactive : RefCounted
{ 
    /// <summary>
    /// <para>Signal emitted on a reactive value being changed</para>
    /// </summary>
    [Signal] public delegate void ReactiveChangedEventHandler(Variant newValue);

    /// <summary>
    /// <para>Subscriber interface for reactive types</para>
    /// <para>Provides subscribers with the changed value as a <see cref="Godot.Variant"/> parameter</para>
    /// </summary>
    public event Action<Variant> OnChanged
    {
        add => Connect(SignalName.ReactiveChanged, Callable.From(value));
        remove => Disconnect(SignalName.ReactiveChanged, Callable.From(value)); 
    }
    
}

/// <summary>
/// <para><see cref="GoldenHand.Data.Reactive"/> type which wraps a <see cref="Godot.Variant"/></para>
/// <para>Emits <see cref="GoldenHand.Data.Reactive.ReactiveChanged"/> signal when encapsulated variable is set</para>
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class ReactiveVariant<[MustBeVariant] T> : Reactive
{

    private T reactiveValue; 
    public T Value
    {
        get => reactiveValue;
        set
        { 
            reactiveValue = value; 
            EmitSignal(SignalName.ReactiveChanged, Variant.From(reactiveValue));
        }
    }

}

// quick and dirty dynamic array
// proper implementation to be considered as demanded by project

/// <summary>
/// <para><see cref="GoldenHand.Data.Reactive"/> dynamic array</para>
/// <para>Emits <see cref="GoldenHand.Data.Reactive.ReactiveChanged"/> signal when operating on array</para> 
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class ReactiveArray<[MustBeVariant] T> : Reactive
{
    private int capacity;
    private int size;
    private T?[] reactiveValues;
    
    public T Last => (reactiveValues[size - 1] ?? default) ?? throw new InvalidOperationException();
    public int Count => size;

    public ReactiveArray()
    {
        size = 0;
        capacity = 0;
        reactiveValues = new T[capacity];
    }
    public ReactiveArray(int desiredSize)
    {
        size = desiredSize;
        capacity = size * 2;
        reactiveValues = new T[capacity];
    }
    public ReactiveArray(T[] values)
    { 
        size = values.Length;
        capacity = size * 2;
        reactiveValues = new T[capacity];
        values.CopyTo(reactiveValues, 0);
    }

    public T this[int i]
    {
        get => (reactiveValues[i] ?? default) ?? throw new InvalidOperationException();
        set
        {
            if (!IsWithinSize(i))
                throw new ArgumentOutOfRangeException(nameof(i));
            
            reactiveValues[i] = value;
            EmitSignal(SignalName.ReactiveChanged, Variant.From(reactiveValues[i]));
        }
    } 

    /// <summary>
    /// <para>Adds the given element to the end of the reactive array</para>
    /// <para>Emits an <see cref="GoldenHand.Data.Reactive.ReactiveChanged"/> signal</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Add(T value)
    {
        if (!IsWithinSize(size))
            throw new ArgumentOutOfRangeException(nameof(size));
        if (size == capacity)
            Resize();
        size++;
        reactiveValues[size - 1] = value;
        EmitSignal(SignalName.ReactiveChanged, Variant.From(reactiveValues[size]));
    }
    
    /// <summary>
    /// <para>Removes an element at the given index</para>
    /// <para>Emits an <see cref="GoldenHand.Data.Reactive.ReactiveChanged"/> signal</para>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void RemoveAt(int i)
    {
       if (!IsWithinSize(i)) 
           throw new ArgumentOutOfRangeException(nameof(i));
       if (!IsWithinSize(size - 1))
           throw new ArgumentOutOfRangeException(nameof(i));

       if (IsWithinSize(i + 1)) 
           reactiveValues.AsSpan(i + 1).ToArray().CopyTo(reactiveValues,i);
       else
           reactiveValues[i] = default;
       
       size--;
       
       EmitSignal(SignalName.ReactiveChanged, Variant.From(reactiveValues[i]));
    }

    private bool IsWithinSize(int index)
    {
        if (index < 0 || index >= size)
        {
            return false;
        }
        return true;
    }

    private void Resize()
    {
        if (size != capacity)
            return;
        
        capacity *= 2; 
        T[] newValues = new T[capacity]; 
        reactiveValues.CopyTo(newValues, 0);
        reactiveValues = newValues; 

    }

    
    
}