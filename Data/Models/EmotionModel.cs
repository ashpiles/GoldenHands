using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Godot;
using Godot.Collections;
using GoldenHand.Data.Types;

namespace GoldenHand.Data.Models;



[GlobalClass, Tool]
public partial class EmotionModel : GameStateModel
{
    public const int EmotionCount = 7;
 
    private float[] attitudes = new float[EmotionCount];
    [Export] private Emotions mask;

    [Export]
    public Array<float> ExposedAttitudes
    {
        get
        {
            var arr = new Array<float>();
            arr.AddRange(attitudes);
            return arr;
        }
        set
        {
            for (int i = 0; i < Mathf.Min(value.Count, attitudes.Length); i++)
            {
                if (!Has(mask, i))
                    attitudes[i] = Mathf.Clamp(value[i], -1f, 1f);
            }
        }
    }
 
    public float this[int index]
    {
        get => attitudes[index];
        set
        {
            if(!Has(mask, index))
                attitudes[index] = Mathf.Clamp(value, -1f, 1f);
        }
    }

    public IEnumerable<float> this[Emotions emotions]
    {
        get => ToIndices(emotions).Select(index => attitudes[index]);
        set
        { 
            foreach (var( index, attitude) in ToIndices(emotions).Zip(value))
            {
                if (!Has(mask, index))
                    attitudes[index] = Math.Clamp(attitude, -1f, 1f);
            }
        }
    }

    public static float Strain(EmotionModel a, EmotionModel b)
    {
        var strain = 0f;
        for (var index = 0; index < EmotionCount; index++)
            strain += a[index] -  b[index];
        
        return strain;
    }

    public void ApplyMask(params Emotions[] emotions)
    {
        mask = emotions.Aggregate((acc, em) =>  acc | em);
    }

    public static EmotionModel operator +(EmotionModel a, float[] b) => Map(a, b, (x, y) => x + y);
    public static EmotionModel operator -(EmotionModel a, float[] b) => Map(a, b, (x, y) => x - y);
    
    public static int ToIndex(Emotions e) => BitOperations.TrailingZeroCount((int)e);

    public static IEnumerable<int> ToIndices(Emotions mask)
    {
        uint bits = (uint)mask;
        while (bits != 0)
        {
            yield return BitOperations.TrailingZeroCount(bits);
            bits &= bits - 1;
        }
    }
    private static EmotionModel Map(EmotionModel a, float[] b, Func<float, float, float> op)
    {
        if (b.Length < EmotionCount)
            throw new ArgumentOutOfRangeException();
        
        var r = new  EmotionModel();
        for (var i = 0; i < EmotionCount; i++)
        {
            if (Has(a.mask, i))
                continue;
            a[i] = op(a[i], b[i]); 
        }
        
        return r;
    }

    private static bool Has(Emotions mask, int index) => ((int)mask & (1 << index)) != 0; 

}