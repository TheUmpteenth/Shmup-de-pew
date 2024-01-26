using System.Collections.Generic;
using UnityEngine;

public class PoolsManager
{
    public static readonly Vector3 k_hiddenPos = new Vector3(100, 100, 100);
    
    private static PoolsManager m_instance;

    private int m_poolsInitialisaing = 0;

    public static PoolsManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new PoolsManager();
            }
            return m_instance;
        }
    }

    private Dictionary<string, PrefabPool> m_activePools = new();

    public void CreatePools()
    {
        //ideally this takes a config file and creates the pools for an individual level
        //here, we'll hard code.
        m_activePools.Clear();
        CreatePool("Assets/Prefabs/Bullet.prefab", "Bullet", 100);
        CreatePool("Assets/Prefabs/Enemy.prefab", "Enemy", 10);
    }

    public bool ArePoolsReady()
    {
        return m_poolsInitialisaing == 0;
    }

    public bool TryGetPool(string poolName, out PrefabPool pool)
    {
        return m_activePools.TryGetValue(poolName, out pool);
    }

    public void ReturnObject(string poolName, GameObject returnee)
    {
        if (!TryGetPool(poolName, out var pool))
        {
            Debug.Log($"Oh Noes! {poolName} is not a valid pool");
        }
        pool.ReturnToPool(returnee);
    }

    private void CreatePool(string address, string poolName, int initialSize, bool expandable = false)
    {
        ++m_poolsInitialisaing;
        //TODO: closure - re-architect
        AssetManager.Instance.RequestAsset<GameObject>(address, obj =>
        {
            var pool = new PrefabPool();
            pool.Setup(obj, initialSize, expandable);
            m_activePools.Add(poolName, pool);
            --m_poolsInitialisaing;
        });
    }
}