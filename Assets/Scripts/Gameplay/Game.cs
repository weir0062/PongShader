using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour {
    // Use this for initialization

    Material shader;
    public Vector3 BallPos;
    public Vector3 PaddlePos;
    bool IsGameOn;
     float DefaultPaddleY = -16.9f;
     float DefaultPaddleX = 0;
    public AudioSource audioSource;
    public List<AudioClip> soundsList;
    int randSoundToSet;
    void Start ()
    {
        // Construct Ball and Paddle classes
        
        
        shader = GetComponent<Renderer>().material;
        m_Ball = new Ball(-33, 20, 20, -20, shader.GetFloat("_DotRadius"), audioSource, soundsList);
        m_Paddle = new Paddle(ref m_Ball, new Vector2(0, -3.69f), -33, 20, shader.GetFloat("_RectHeight"), shader.GetFloat("_RectWidth"));
        m_Paddle.SetPosition(new Vector2(DefaultPaddleX, DefaultPaddleY));
        IsGameOn = false;
    }

    // Update is called once per frame
    void Update ()
    {
        IsGameOn = m_Ball.GetIsBallMoving();



        if(IsGameOn)
        {

        m_Ball.Update();
        m_Paddle.Update();
        }
        else
        {
            m_Paddle.SetPosition(new Vector2(DefaultPaddleX, DefaultPaddleY));
        }
        BallPos= m_Ball.GetPosition();
        PaddlePos=m_Paddle.GetPosition();

        

        shader.SetVector("_Position", BallPos);
        shader.SetVector("_RectCentre", PaddlePos);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Ball.StartGame();
            
        }


    }

    Ball m_Ball;
    Paddle m_Paddle;
}
