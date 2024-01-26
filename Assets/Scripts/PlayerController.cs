using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float m_speed = 10f;
        [SerializeField] private float m_shootCooldown = 0.3f;

        private enum eActiveInput
        {
            Fore = 1 << 0,
            Back = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
            Fire = 1 << 4
        }

        private int m_activeInput = 0;

        private void Update()
        {
            UpdateMovement();
            m_activeInput = 0;
        }

        private void UpdateMovement()
        {
            Vector2 movement = Vector2.zero;
            if ((m_activeInput & (int)eActiveInput.Fore) != 0)
            {
                movement.y += 1;
            }
            if ((m_activeInput & (int)eActiveInput.Back) != 0)
            {
                movement.y -= 1;
            }
            if ((m_activeInput & (int)eActiveInput.Right) != 0)
            {
                movement.x += 1;
            }
            if ((m_activeInput & (int)eActiveInput.Left) != 0)
            {
                movement.x -= 1;
            }

            movement *= Time.deltaTime * m_speed;

            var oldPos = transform.position;
            var newPos = new Vector3(oldPos.x + movement.x, oldPos.y + movement.y, oldPos.z);
            transform.position = newPos;
        }

        private void Awake()
        {
            InputController.OnForwardPressed += ForwardRequested;
            InputController.OnBackPressed += ReverseRequested;
            InputController.OnLeftPressed += LeftStrafeRequested;
            InputController.OnRightPressed += RightStrafeRequested;
            InputController.OnFirePressed += GetComponent<Shooter>().FireRequested;
        }

        private void OnDestroy()
        {
            InputController.OnForwardPressed -= ForwardRequested;
            InputController.OnBackPressed -= ReverseRequested;
            InputController.OnLeftPressed -= LeftStrafeRequested;
            InputController.OnRightPressed -= RightStrafeRequested;
            InputController.OnFirePressed -= GetComponent<Shooter>().FireRequested;
        }

        private void ForwardRequested()
        {
            m_activeInput |= (int)eActiveInput.Fore;
        }

        private void ReverseRequested()
        {
            m_activeInput |= (int)eActiveInput.Back;
        }

        private void LeftStrafeRequested()
        {
            m_activeInput |= (int)eActiveInput.Left;
        }

        private void RightStrafeRequested()
        {
            m_activeInput |= (int)eActiveInput.Right;
        }
    }
}