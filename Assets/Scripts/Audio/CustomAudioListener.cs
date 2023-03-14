using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;

public class CustomAudioListener : MonoBehaviour
{
    public float Panning;

    void Start()
    {
        if (GetComponent<AudioListener>() == null)
        {
            gameObject.AddComponent<AudioListener>();
        }
       
        //The audio manager needs to be aware of every listener
        AudioZoneGroup = AudioManager.Instance.DefaultAudioZoneGroup;
        AudioManager.Instance.RegisterListener(this);
        m_PrevPosition = transform.position;
    }

    void OnDestroy()
    {
        AudioManager.Instance.UnregisterListener();
    }

    void LateUpdate()
    {
        m_PrevPosition = transform.position;
    }

    //Calculates 3D volume using the specified falloff function
    public float Calc3DVolume(FalloffFunc falloffFunc, Vector3 sourcePosition)
    {
        float dist = Vector3.Distance(sourcePosition, transform.position);

        return falloffFunc.CalcFalloff(dist);
    }
    
    //Calculates the pitch shift applied from the doppler effect.  This code is partly based on the
    //formulas provided here:  http://en.wikipedia.org/wiki/Doppler_effect
    public float CalcDopplerPitch(Vector3 sourceVelocity, Vector3 sourcePosition, float dopplerLevel)
    {
        Vector3 sourceDir = sourcePosition - transform.position;
        float sourceDist = sourceDir.magnitude;

        if (sourceDist <= 0.0f)
        {
            return 1.0f;
        }

        sourceDir /= sourceDist;

        float sourceSpeedAlongDir = Vector3.Dot(sourceDir, sourceVelocity);

        float listenerSpeedAlongDir = Vector3.Dot(sourceDir, GetVelocity());

        float relativeSpeed = listenerSpeedAlongDir - sourceSpeedAlongDir;

        float pitchShift = 1.0f + (relativeSpeed / AudioManager.Instance.SpeedOfSound) * dopplerLevel;

        pitchShift = Mathf.Max(pitchShift, 0.0f);

        return pitchShift;
    }

    public Vector3 GetVelocity()
    {
        Vector3 movementThisFrame = transform.position - m_PrevPosition;

        return movementThisFrame /= Time.deltaTime;
    }

    public AudioMixerGroup AudioZoneGroup { get; set; }
   

    //The previous position is tracked so we can calculate the objects velocity
    //(This is used for the doppler effect)
    Vector3 m_PrevPosition;
}