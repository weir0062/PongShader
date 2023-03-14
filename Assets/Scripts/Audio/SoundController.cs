using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;

//This class is meant to extend the behaviour of the Unity AudioSource
public class SoundController : MonoBehaviour
{
    //This will be set up automatically in start if it is left NULL.
    //The main purpose of this is to allow you to manually specify the source if you have multiple
    //AudioSources on the same object
    public AudioSource SoundSource;
    public SoundEffect SoundEffect;

    public FalloffFunc FalloffFunc;

    public float DopplerLevel = 1.0f;

    public bool PlayOnStart = false;
    public bool Loop = false;

    void Start()
    {
        //If the given sound source is null create a new one to use
        if (SoundSource == null)
        {
            SoundSource = gameObject.AddComponent<AudioSource>();
        }

        SoundSource.spatialBlend = 0.0f;
        SoundSource.loop = Loop;

        AudioManager.Instance.RegisterController(this);

        m_ExternalVolume = 1.0f;

        SoundSource.volume = CalcVolume(SoundEffect);

        m_PrevPosition = transform.position;

        if (PlayOnStart)
        {
            Play();
        }
    }

    void OnDestroy()
    {
        AudioManager.Instance.UnregisterController(this);
    }

    void OnDisable()
    {
        AudioManager.Instance.UnregisterController(this);
    }

    void Update()
    {
        UpdateListener();

        SoundSource.volume = CalcVolume(SoundEffect);
    }

    void LateUpdate()
    {
        m_PrevPosition = transform.position;
    }

    public void PlayOneShot()
    {
        PlayOneShot(SoundEffect);
    }

    public void PlayOneShot(SoundEffect effect)
    {
        //Choose the clip to play
        AudioClip clip = effect.GetClipToPlay();

        //Update the listener and volume
        UpdateListener();

        float volume = CalcVolume(effect);

        //Play sound
        SoundSource.PlayOneShot(clip, volume);
    }

    public void Play()
    {
        //Choose the clip to play
        if (SoundEffect != null)
        {
            SoundSource.clip = SoundEffect.GetClipToPlay();
        }

        //Update the listener and volume
        UpdateListener();

        SoundSource.volume = CalcVolume(SoundEffect);

        //Play the sound
        SoundSource.Play();
    }

    public void Pause()
    {
        SoundSource.Pause();
    }

    public void Stop()
    {
        SoundSource.Stop();
    }

    public void SetVolume(float volume)
    {
        m_ExternalVolume = volume;

        SoundSource.volume = CalcVolume(SoundEffect);
    }

    public Vector3 GetVelocity()
    {
        Vector3 movementThisFrame = transform.position - m_PrevPosition;

        return movementThisFrame /= Time.deltaTime;
    }

    void UpdateListener()
    {
       CustomAudioListener listener = AudioManager.Instance.GetListener();

        if (listener == null)
        {
            return;
        }

        SoundSource.panStereo = listener.Panning;

        SoundSource.pitch = listener.CalcDopplerPitch(
            GetVelocity(),
            transform.position,
            DopplerLevel
            );

        m_3DVolume = listener.Calc3DVolume(FalloffFunc, transform.position);

        SoundSource.outputAudioMixerGroup = listener.AudioZoneGroup;
    }

    //This function applies all of the volumes that effect the sound.  This keeps all of the
    //volumes that need to be applied in one spot which helps to reduce bugs.
    float CalcVolume(SoundEffect effect)
    {
        float volume =
            m_ExternalVolume *
            m_3DVolume;

        if (effect != null)
        {
            volume *= effect.Volume;
        }

        return volume;
    }

    float m_ExternalVolume;
    float m_3DVolume;

    //The previous position is tracked so we can calculate the objects velocity
    //(This is used for the doppler effect)
    Vector3 m_PrevPosition;
}
