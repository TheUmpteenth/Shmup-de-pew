using UnityEngine;

public class GoForwards : MonoBehaviour
{
    public float m_speed = 10f;
    
    void Update()
    {
        transform.position += m_speed * Time.deltaTime * transform.up;
    }
}
