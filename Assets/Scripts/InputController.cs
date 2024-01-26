using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    //Input event system might be overkill, but separation of concerns.
    public static event Action OnForwardPressed;
    public static event Action OnBackPressed;
    public static event Action OnLeftPressed;
    public static event Action OnRightPressed;
    public static event Action OnFirePressed;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            OnForwardPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            OnLeftPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            OnBackPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            OnRightPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            OnFirePressed?.Invoke();
        }
    }
}
