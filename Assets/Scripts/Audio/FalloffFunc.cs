using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class FalloffFunc
{
    public enum FuncType
    {
        Linear,
        InverseSquared,
        Power
    }

    //The type of function to use
    public FuncType Type = FuncType.Power;

    //The parameters for the falloff function.  Note that not every parameter will be used
    //for every parameter type.
    public float MinDist = 1.0f;
    public float MaxDist = 30.0f;
    public float Power = 2.0f;

    public FalloffFunc()
    {
    }

    public float CalcFalloff(float dist)
    {
        switch (Type)
        {
            case FuncType.Linear:
                return CalcLinearFalloff(dist);

            case FuncType.InverseSquared:
                return CalcInverseSquaredFalloff(dist);

            case FuncType.Power:
                return CalcPowerFalloff(dist);

            default:
                DebugUtils.LogError("Invalid Falloff type: {0}", Type);
                return 0.0f;
        }
    }

    //This will decrease linearly starting at MinDist and hitting zero at MaxDist.
    float CalcLinearFalloff(float dist)
    {
        float falloff = Mathf.InverseLerp(MinDist, MaxDist, dist);

        falloff = 1.0f - Mathf.Clamp01(falloff);

        return falloff;
    }

    //This a natural sounding falloff, but but it will take a really long distance for it 
    //to become inaudible.  This makes this function less practical to use in a game.
    float CalcInverseSquaredFalloff(float dist)
    {
        float falloff = 1.0f / (dist + 1 - MinDist);

        falloff = Mathf.Clamp01(falloff);

        return falloff;
    }

    //This will decrease using a power function starting at MinDist and hitting zero at MaxDist.
    //A higher power will cause the volume to accelerate to zero faster.  This is the most
    //versatile function written here and will probably be used the most frequently.
    float CalcPowerFalloff(float dist)
    {
        dist = Mathf.Clamp(dist, MinDist, MaxDist);

        float falloff = (-dist + MaxDist) / (MaxDist - MinDist);

        falloff = Mathf.Pow(falloff, Power);

        return falloff;
    }
}
