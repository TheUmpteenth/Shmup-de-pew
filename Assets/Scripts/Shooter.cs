using DefaultNamespace;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private BulletLauncherConfig.BulletLauncher m_configuration;
    private bool m_isPlayer = false;

    private void Awake()
    {
        m_isPlayer = GetComponent<PlayerController>() != null;
    }

    public void SetConfiguration(BulletLauncherConfig.BulletLauncher configuration)
    {
        m_configuration = configuration;
    }

    public void FireRequested()
    {
        var position = transform.position;
        foreach (var launcher in m_configuration.m_launchers)
        {
            BulletManager.CreateBullet(position, launcher, m_isPlayer);
        }
    }
}
