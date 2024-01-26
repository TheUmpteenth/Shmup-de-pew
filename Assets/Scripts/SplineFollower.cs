using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    private Spline m_spline;
    private float m_delay;
    private float m_speed;
    private float m_splineTime;
    private bool m_stopped;

    public event Action<SplineFollower> onSplineEnd;

    public void Setup(Spline spline, float speed, float delay)
    {
        m_spline = spline;
        m_delay = delay;
        m_speed = (1f / m_spline.GetLength()) * speed;
        m_stopped = false;
        m_splineTime = 0f;
    }

    private void Update()
    {
        if (m_delay > 0f)
        {
            m_delay -= Time.deltaTime;
            return;
        }

        if (m_stopped) return;

        if (!m_spline.Closed && m_splineTime >= 1f)
        {
            m_stopped = true;
            onSplineEnd?.Invoke(this);
            return;
        }

        float3 pos = m_spline.EvaluatePosition(m_splineTime);

        transform.position = new Vector3(pos.x, pos.y, pos.z);

        m_splineTime += m_speed * Time.deltaTime;
    }
}