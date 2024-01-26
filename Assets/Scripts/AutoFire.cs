using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class AutoFire : MonoBehaviour
{
    [SerializeField]
    private List<float> m_delays = new();

    private int m_delayIndex = 0;
    private float m_currentTime = 0f;
    private float m_currentDelay = 0f;

    private event Action onRequestFire;

    public void SetConfiguration(BulletLauncherConfig.AutoFireConfig config)
    {
        m_delays = config.m_delays;
    }

    private void Awake()
    {
        onRequestFire += GetComponent<Shooter>().FireRequested;
    }
    
    private void OnDestroy()
    {
        onRequestFire -= GetComponent<Shooter>().FireRequested;
    }

    private void Update()
    {
        m_currentTime += Time.deltaTime;
        if (m_currentTime >= m_currentDelay)
        {
            m_currentTime -= m_currentDelay;
            m_currentDelay = m_delays[++m_delayIndex % m_delays.Count];
            onRequestFire?.Invoke();
        }
    }
}
