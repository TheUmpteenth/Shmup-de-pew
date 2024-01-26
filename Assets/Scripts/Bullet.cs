using DefaultNamespace;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool m_playerOwns;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet) return;
        
        var player = other.GetComponent<PlayerController>();
        
        if (m_playerOwns)
        {
            if (player != null)
            {
                return;
            }
            
            PoolsManager.Instance.ReturnObject("Enemy", other.gameObject);
            ScoreManager.AddScore(10);
        }
        else
        {
            if (player == null)
            {
                return;
            }
            
            Destroy(other.gameObject);
            LivesManager.RemoveLife();
        }

        BulletManager.ReturnBullet(gameObject);
    }
}
