using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Godot;

namespace GoldenHand.Data.Types;

public enum EmotionType
{
    Happiness,
    Sadness,
    Surprise,
    Fear,
    Anger,
    Disgust,
    Contempt
}

/// <summary>
/// <para>Simple type denoting a range of -1 to 1</para>
/// </summary>
public readonly struct Attitude
{
    public float Value { get; }
    public Attitude(float value) => Value = Math.Clamp(value, -1f, 1f);

    public static Attitude operator +(Attitude a, float delta) => new(a.Value + delta);
    public static Attitude operator -(Attitude a, float delta) => new(a.Value - delta);
    
    public static explicit operator float(Attitude attitude) => attitude.Value;
    public static explicit operator Attitude(float value) => new (value);
}
 
/// <summary>
/// <para>Following the ABX Communication Model this type outlines the attitude towards a context, and the attraction towards the other party as a matrix of emotions</para>
/// <para><see cref="GoldenHand.Data.Types.EmotionType"/> denotes the emotions with <see cref="GoldenHand.Data.Types.Attitude"/> representing the weight of said emotion. This provides a simple yet deep representation of how a character feels about a topic in relation to another party.</para>
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class ABXContext : RefCounted
{  
    public Dictionary<EmotionType, Attitude> ContextAttitude = new();
    public Dictionary<EmotionType, Attitude> AttitudeTowardsOther = new(); 
    
    //TODO: Add some sort of matrix multiplication, or consider re-doing current implementation to be a true matrix
  

}