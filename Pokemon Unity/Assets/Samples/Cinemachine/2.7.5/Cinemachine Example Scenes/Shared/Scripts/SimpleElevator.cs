using UnityEngine;

/// <summary>
/// Moves transform between minY and maxY with the specified speed.
/// </summary>
public class SimpleElevator : MonoBehaviour
{
    public float minY, maxY;
    public float speed;
    public bool on;

    float m_Direction = 1;
    void FixedUpdate()
    {
        if (transform.position.y < minY)
        {
            m_Direction = 1f;
        }

        if (transform.position.y > maxY)
        {
            m_Direction = -1f;
        }
        
        var dir = new Vector3(0, m_Direction * speed * Time.fixedDeltaTime, 0);
        transform.position += dir;
    }
}
