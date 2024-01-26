using UnityEngine;

public class Poolable : MonoBehaviour
{
    private bool m_active = true;
    
    public virtual void ReturnToPool()
    {
        if (m_active)
        {
            m_active = false;
            //using set active here might be costly as it'll call OnDisable on child objects. Override this to control individual components
            gameObject.SetActive(false);
            transform.position = PoolsManager.k_hiddenPos;
        }
        else
        {
            Debug.Log("Oh noes! Inactive Poolable being returned");
        }
    }

    public virtual void ActivateFromPool()
    {
        if (!m_active)
        {
            m_active = true;
            //using set active here might be costly as it'll call OnEnable on child objects. Override this to control individual components
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Oh noes! Active Poolable being depooled");
        }
    }
}
