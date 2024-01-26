using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletLauncherConfig", menuName = "ScriptableObjects/BulletLauncherConfig", order = 1)]
public class BulletLauncherConfig : ScriptableObject
{
    [Serializable]
    public struct IndividualLauncher
    {
        public Vector2 m_positionOffset;
        public Vector2 m_direction;
    }

    [Serializable]
    public class BulletLauncher
    {
        public List<IndividualLauncher> m_launchers = new();
    }

    [Serializable]
    public class AutoFireConfig
    {
        public List<float> m_delays = new();
    }

    public List<BulletLauncher> m_launcherTypes = new();

    [SerializeField]
    public List<AutoFireConfig> m_autoFireTypes = new();
}
