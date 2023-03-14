using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;


public class AudioManager : MonoBehaviour 
{
    public float SpeedOfSound = 343.2f;  //Approximate speed of sound in meters per second

    public AudioMixerGroup DefaultAudioZoneGroup;

    void Awake()
    {
        //This is similar to a singleton in that it only allows one instance to exist and there is instant global 
        //access to the GameManager using the static Instance member.
        //
        //This will set the instance to this object if it is the first time it is created.  Otherwise it will delete 
        //itself.
        if (Instance == null)
        {
            //This tells unity not to delete the object when you load another scene
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_SoundControllers = new List<SoundController>();
    }

    void Update()
    {
    }

    public static AudioManager Instance { get; private set; }


    //This is used to keep track of all of the sound controllers, so we
    //can update them if the sound volumes change
    public void RegisterController(SoundController controller)
    {
        m_SoundControllers.Add(controller);
    }

    public void UnregisterController(SoundController controller)
    {
        m_SoundControllers.Remove(controller);
    }
    public void SetListener(CustomAudioListener listener)
    {
        m_Listener = listener;
    }

    public void RegisterListener(CustomAudioListener listener)
    {
        m_Listener = listener;
    }

    public void UnregisterListener()
    {
        m_Listener = null;
    }

    public CustomAudioListener GetListener()
    {
        return m_Listener;
    }

    List<SoundController> m_SoundControllers;

    CustomAudioListener m_Listener;
}


