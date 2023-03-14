using UnityEngine;

public class Paddle
{


    public Paddle(ref Ball ball, Vector2 position, float leftWallPos, float rightWallPos, float width, float height)
    {
      m_Ball = ball;
        m_InitialPosition = m_Position = position;
      m_PaddleSpeed = 16.9f;
      m_PaddleWidth = width;
      m_PaddleWidth = height;
      m_LeftWallPos = leftWallPos + m_PaddleWidth/2;
      m_RightWallPos = rightWallPos - m_PaddleWidth/2;
        
    }

    public void Update()
    {
      if(Input.GetKey(KeyCode.A))
      {
            m_Position.x -= m_PaddleSpeed * Time.deltaTime;
      }

      if(Input.GetKey(KeyCode.D))
      {
            m_Position.x += m_PaddleSpeed * Time.deltaTime;
      }
        m_Position.x = Mathf.Clamp(m_Position.x, -33, 20);
        
      Vector2 pointOfCollision;
      //Collision Check
      if(CheckCollision(m_Ball, out pointOfCollision))
      {
          //Collision Response
          Vector2 paddleNormal = new Vector2(0, -1);
          m_Ball.SetPosition(new Vector2(pointOfCollision.x, pointOfCollision.y + m_Ball.GetRadius()));
          m_Ball.SetVelocity(-2.0f * Vector2.Dot(m_Ball.GetVelocity(), paddleNormal) * paddleNormal + m_Ball.GetVelocity());
            m_Ball.PlayRandomSound();
      }



    }
    bool CheckCollision(Ball ball, out Vector2 pointOfCollision)
    {
        
        //Ensure the ball is moving
        if (ball.GetVelocity() != Vector2.zero)
        {
            //Calculate the closest point of the line
            Vector2 lineStart = new Vector2(m_Position.x - m_PaddleWidth / 2, m_Position.y);
            Vector2 lineEnd = new Vector2(m_Position.x + m_PaddleWidth / 2, m_Position.y);
            Vector2 closestPoint = CalculateClosestPoint(m_Ball.GetPosition(), m_Ball.GetRadius(), lineStart, lineEnd);
            pointOfCollision = closestPoint;
            //Calculate the distance between the closest point and the center of the ball
            float distance = Vector2.Distance(ball.GetPosition(), closestPoint);
            float radius = ball.GetRadius();

            //If the distance squared is less than the radii squared, then there's a collision
            bool didCollide = distance < radius;
            return didCollide;
        }
        pointOfCollision = Vector2.zero;
        return false;
    }
    
    bool MyCheckCollision(Ball ball, out Vector2 pointOfCollision)
    {

        //Ensure the ball is moving
        if (ball.GetVelocity() != Vector2.zero)
        {
            Vector2 lineStart = new Vector2(m_Position.x - m_PaddleWidth / 2, m_Position.y);
             Vector2 lineEnd = new Vector2(m_Position.x + m_PaddleWidth / 2, m_Position.y);
             Vector2 closestPoint = CalculateClosestPoint(m_Ball.GetPosition(), m_Ball.GetRadius(), lineStart, lineEnd);
             pointOfCollision = closestPoint;
             //Calculate the distance between the closest point and the center of the ball
             float distance = Vector2.Distance(ball.GetPosition(), closestPoint);
             float radius = ball.GetRadius();

            
            if (m_Ball.GetPosition().x+m_Ball.GetRadius() < m_Position.x + m_PaddleWidth / 2 &&
             m_Ball.GetPosition().x+m_Ball.GetRadius() > m_Position.x - m_PaddleWidth / 2 &&
             m_Ball.GetPosition().y+m_Ball.GetRadius() < m_Position.y + m_PaddleHeight / 2 &&
             m_Ball.GetPosition().y + m_Ball.GetRadius() > m_Position.y - m_PaddleHeight / 2)
            {
                return true;

            }

        }
        pointOfCollision = Vector2.zero;
        return false;
    }
    Vector2 CalculateClosestPoint(Vector2 aCircleCenter, float aRadius, Vector2 aLineStart, Vector2 aLineEnd)
    {
        //Calculate the circle vector        
        Vector2 circleVector = aCircleCenter - aLineStart;

       //Calculate the line segment vector        
       Vector2 lineVector = aLineEnd - aLineStart;

        //Normalize the line segment vector        
       Vector2 normalizedVector = lineVector.normalized;

        //Calculate the dot product between the circle vector and the normalized line segment vector       
       float magnitude = Vector2.Dot(normalizedVector, circleVector);

       //Calculate the projection using the result of the dot product and multiply it by the normalized line segment        
       Vector2 projection = normalizedVector * magnitude;

       //Calculate the closest point on the line segment, by adding the project vector to the line start vector        
       Vector2 closestPoint = aLineStart + projection;
       closestPoint.x = Mathf.Clamp(closestPoint.x, aLineStart.x, aLineEnd.x);
       closestPoint.y = Mathf.Clamp(closestPoint.y, aLineStart.y, aLineEnd.y);
       return closestPoint;
    }
    public void SetPosition(Vector2 pos)
    {
        m_Position = pos;
    }
    public Vector2 GetPosition()
    {
        return m_Position;
    }

    Ball m_Ball;
    Vector2 m_Position;
    Vector2 m_InitialPosition;
    float m_PaddleSpeed;
    float m_LeftWallPos;
    float m_RightWallPos;
    float m_PaddleWidth;
    float m_PaddleHeight;
}
