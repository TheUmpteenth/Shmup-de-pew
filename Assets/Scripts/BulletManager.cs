using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager Instance { get; set; } // don't give access to the singleton - it's just that we don't want two of this.
    private void Awake() 
    { 
        // If there is an instance, and it's not this, delete this.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    private class RegisteredBullet
    {
        public GameObject m_bullet;
        public float m_lifetime;
    }

    private static PrefabPool m_bulletPool;
    private static List<RegisteredBullet> m_registeredBullets = new();
    private static List<RegisteredBullet> m_deregisteringBullets = new();
    
    public float m_bulletLifetime = 1f;
    public float m_bulletSpeed = 10f;

    public static void Setup()
    {
        if (!PoolsManager.Instance.TryGetPool("Bullet", out m_bulletPool))
        {
            Debug.Log("Oh noes! no bullet pool exists!");
        }
    }

    public void Update()
    {
        foreach (var bullet in m_deregisteringBullets)
        {
            Deregister(bullet);
        }

        foreach (var bullet in m_registeredBullets)
        {
            bullet.m_lifetime += Time.deltaTime;
            if (bullet.m_lifetime >= m_bulletLifetime)
            {
                m_deregisteringBullets.Add(bullet);
                m_bulletPool.ReturnToPool(bullet.m_bullet);
                continue;
            }

            if (bullet.m_bullet.activeSelf == false)//handle hit bullets
            {
                m_deregisteringBullets.Add(bullet);
                continue;
            }
            
            bullet.m_bullet.transform.position += m_bulletSpeed * Time.deltaTime * bullet.m_bullet.transform.up;
        }
    }

    public static void ReturnBullet(GameObject bullet)
    {
        m_bulletPool.ReturnToPool(bullet);
    }

    public static void CreateBullet(Vector3 creatorsPosition, BulletLauncherConfig.IndividualLauncher launcher,
        bool playerOwns = false)
    {
        if (!m_bulletPool.TryGetPooledObject(out var bullet))
        {
            Debug.Log("Oh noes! can't create bullet!");
        }

        bullet.GetComponent<Bullet>().m_playerOwns = playerOwns;
        
        bullet.transform.position = new Vector3(creatorsPosition.x + launcher.m_positionOffset.x,
            creatorsPosition.y + launcher.m_positionOffset.y, creatorsPosition.x);
        bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, launcher.m_direction);

        Register(bullet);
    }

    private static void Register(GameObject bullet)
    {
        var newBullet = new RegisteredBullet() { m_bullet = bullet, m_lifetime = 0f };
        m_registeredBullets.Add(newBullet);
    }

    private void Deregister(RegisteredBullet bullet)
    {
        m_registeredBullets.Remove(bullet);
    }
}