using System.Collections.Generic;
using UnityEngine;

public class Ball
{

    public AudioSource audioSource;
    public List<AudioClip> soundsList;
    public Ball(float leftWallPos, float rightWallPos, float topWallPos, float bottomWallPos, float radius, AudioSource audio, List<AudioClip> sounds)
    {
        ResetBall();
        m_Speed = 16.9f;

        m_Velocity = Vector2.zero;
       
        m_LeftWallPos = leftWallPos;
        m_RightWallPos = rightWallPos;
        m_TopWallPos = topWallPos;
        m_BottomWallPos = bottomWallPos;
        m_Radius = radius;
        IsBallMoving = false;

        audioSource = audio;
        soundsList = sounds;


    }
    Vector2 GenerateRandomDirection()
    {
        float angle = Random.Range(20.0f, 160.0f);
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }
    public void PlayRandomSound()
    {
        int randSoundToSet = Random.Range(0, soundsList.Count - 1);
        audioSource.clip = soundsList[randSoundToSet];
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void ResetBall()
    {
        m_WallNormal = new Vector2(0, 0);
        m_Position = new Vector2(0, 0);
    }
    public void StartGame()
    {
        if (IsBallMoving == false)
        {
        m_Velocity = GenerateRandomDirection() * m_Speed;
            IsBallMoving = true;    
        }
    }

    public bool GetIsBallMoving()
    {
        return IsBallMoving;
    }
    void StopGame()
    {
        m_Velocity = Vector2.zero;
        m_WallNormal = Vector2.zero;
        // Reset Ball Position
        m_Position = new Vector2(0, 0);
        IsBallMoving = false;
        PlayRandomSound();
    }

    public void Update()
    {

        m_Position += m_Velocity * Time.deltaTime;

        ////Check For Collision
        if (m_Position.x <= m_LeftWallPos)
        {
            m_WallNormal = new Vector2(-1, 0);
            m_Position.x = m_LeftWallPos;
        }
        else if (m_Position.x >= m_RightWallPos)
        {
            m_WallNormal = new Vector2(1, 0);
            m_Position.x = m_RightWallPos;
        }

        if (m_Position.y >= m_TopWallPos - m_Radius)
        {
            m_WallNormal = new Vector2(0, 1);
            m_Position.y = m_TopWallPos - m_Radius;
        }
        if (m_Position.y <= m_BottomWallPos + m_Radius)
        {
            StopGame();
            ResetBall();
        }

        //Handle Collision
        if (m_WallNormal != Vector2.zero)
        {
            m_Velocity = -2.0f * Vector2.Dot(m_Velocity, m_WallNormal) * m_WallNormal + m_Velocity;
            m_WallNormal = new Vector2(0, 0);
            PlayRandomSound();
        }



    }
    public Vector2 GetPosition()
    {
        return m_Position;
    }
    public void SetPosition(Vector2 position)
    {
        m_Position = position;
    }

    public Vector2 GetVelocity()
    {
        return m_Velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        m_Velocity = velocity;
    }

    public float GetRadius()
    {
        return m_Radius;
    }
    bool IsBallMoving;
    float m_RightWallPos;
    float m_LeftWallPos;
    float m_TopWallPos;
    float m_BottomWallPos;
    float m_Speed;
    float m_Radius;
    Vector2 m_Position;
    Vector2 m_Velocity;
    Vector2 m_WallNormal;
}
