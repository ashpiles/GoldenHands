using System;
using System.Collections.Generic;
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
    [Signal] public delegate void ReactiveChangedEventHandler();
    [Signal] public delegate void ReactiveCreatedEventHandler(Variant newValue);
    [Signal] public delegate void ReactiveReadEventHandler(Variant readValue);
    [Signal] public delegate void ReactiveUpdatedEventHandler(Variant updatedValue);
    [Signal] public delegate void ReactiveDeletedEventHandler(Variant deletedValue);

    /// <summary>
    /// <para>Subscriber interface for reactive types</para>
    /// <para>Provides subscribers with the changed value as a <see cref="Godot.Variant"/> parameter</para>
    /// </summary>
    public event Action OnChanged
    {
        add => Connect(SignalName.ReactiveChanged, Callable.From(value));
        remove => Disconnect(SignalName.ReactiveChanged, Callable.From(value)); 
    } 
    
    public event Action<Variant> OnCreated
    {
        add => Connect(SignalName.ReactiveCreated, Callable.From(value));
        remove => Disconnect(SignalName.ReactiveCreated, Callable.From(value));
    }
    
    public event Action<Variant> OnRead
    {
        add => Connect(SignalName.ReactiveRead, Callable.From(value));
        remove => Disconnect(SignalName.ReactiveRead, Callable.From(value));
    }
    
    public event Action<Variant> OnUpdated
    {
        add => Connect(SignalName.ReactiveUpdated, Callable.From(value));
        remove => Disconnect(SignalName.ReactiveUpdated, Callable.From(value));
    }
    
    public event Action<Variant> OnDeleted
    {
        add => Connect(SignalName.ReactiveDeleted, Callable.From(value));
        remove => Disconnect(SignalName.ReactiveDeleted, Callable.From(value));
    }
    
    
}

 
///  <summary>
/// <para><see cref="GoldenHand.Data.Reactive"/> type which wraps a <see cref="Godot.Variant"/></para>
/// <para>Emits <see cref="GoldenHand.Data.Reactive.ReactiveChanged"/> signal when encapsulated variable is set</para>
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class ReactiveVariant<[MustBeVariant] T> : Reactive
{

    private T? reactiveValue;

    public ReactiveVariant() { }

    public ReactiveVariant(T value)
    {
        reactiveValue = value;
    }

    public static implicit operator T(ReactiveVariant<T> variant) => variant.Value; 
    public static explicit operator ReactiveVariant<T>(T value) => new(value);

    public static bool operator !=(ReactiveVariant<T> a, T b) => !(a.Value.Equals(b)); 
    public static bool operator ==(ReactiveVariant<T> a, T b) => a.Value.Equals(b);

    public T Value
    {
        get => (reactiveValue ?? default) ?? throw new NullReferenceException();
        set
        { 
            reactiveValue = value; 
            EmitSignal(Reactive.SignalName.ReactiveChanged, Variant.From(Value));
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
        capacity = 1;
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
        get
        {
            EmitSignal(Reactive.SignalName.ReactiveRead, Variant.From(reactiveValues[i]));
            return reactiveValues[i] ?? throw new InvalidOperationException();
        }
        set
        {
            if (!IsWithinSize(i))
                throw new ArgumentOutOfRangeException(nameof(i));
            
            reactiveValues[i] = value;
            EmitSignal(Reactive.SignalName.ReactiveChanged, Variant.From(reactiveValues[i]));
            EmitSignal(Reactive.SignalName.ReactiveUpdated, Variant.From(reactiveValues[i]));
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
        EmitSignal(Reactive.SignalName.ReactiveChanged);
        EmitSignal(Reactive.SignalName.ReactiveCreated, Variant.From(value));
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

       EmitSignal(Reactive.SignalName.ReactiveDeleted, Variant.From(reactiveValues[i]));
       
       if (IsWithinSize(i + 1)) 
           reactiveValues.AsSpan(i + 1).ToArray().CopyTo(reactiveValues,i);
       else
           reactiveValues[i] = default;
       
       size--;

       EmitSignal(Reactive.SignalName.ReactiveChanged);
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

public partial class ReactiveDictionary<[MustBeVariant] TKey, [MustBeVariant] TValue> : Reactive
{
    
    private Dictionary<TKey, TValue> reactiveValues;

    public TValue this[TKey key]
    {
        get => reactiveValues[key];
        set => reactiveValues[key] = value;
    }
    
    public bool ContainsKey(TKey key) => reactiveValues.ContainsKey(key);

    public void Add(TKey key, TValue value) => reactiveValues.Add(key, value);
    
}

public partial class ReactiveHashSet<[MustBeVariant] T> : Reactive
{ 
    
}