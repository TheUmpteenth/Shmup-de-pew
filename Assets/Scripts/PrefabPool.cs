using System.Collections.Generic;
using UnityEngine;

public class PrefabPool
{
    
    private GameObject m_basePrefab;
    
    private Queue<GameObject> m_pool = new ();

    private bool expandable = false;

    public void Setup(GameObject prefab, int initialSize, bool allowExpansion = false)
    {
        m_basePrefab = prefab;
        for (int i = 0; i < initialSize; ++i)
        {
            var go = GameObject.Instantiate(m_basePrefab, PoolsManager.k_hiddenPos, Quaternion.identity);
            ReturnToPool(go);
        }

        expandable = allowExpansion;
    }

    public bool TryGetPooledObject(out GameObject pooledObject)
    {
        if (m_pool.TryDequeue(out pooledObject))
        {
            if (pooledObject.TryGetComponent<Poolable>(out var poolable))
            {
                poolable.ActivateFromPool();
            }
            return true;
        }

        if (expandable)
        {
            pooledObject = GameObject.Instantiate(m_basePrefab, PoolsManager.k_hiddenPos, Quaternion.identity);
            return true;
        }

        return false;
    }

    public void ReturnToPool(GameObject pooledObject)
    {
        // need to check the validity of the object in respect to the pool
        if (pooledObject.TryGetComponent<Poolable>(out var poolable))
        {
            poolable.ReturnToPool();
        }
        m_pool.Enqueue(pooledObject);
    }
}