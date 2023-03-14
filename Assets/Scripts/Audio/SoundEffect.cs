using System;
using System.Collections.Generic;
using UnityEngine;

//This class is used to extend the behaviour of the Unity AudioClip
//It allows you to set a separate volume for it, and also specify a
//set of clips to randomly choose from.
[Serializable]
public class SoundEffect
{
    public float Volume = 1.0f;

    public List<AudioClip> Clips;

    public SoundEffect()
    {
        Clips = new List<AudioClip>();
    }

    public AudioClip GetClipToPlay()
    {
        if (Clips.Count <= 0)
        {
            return null;
        }

        //Get a random clip to play
        int randomIndex = UnityEngine.Random.Range(0, Clips.Count);

        return Clips[randomIndex];
    }

    public float GetVolume()
    {
        return Volume;
    }
}
