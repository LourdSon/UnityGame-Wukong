using UnityEngine;
using UnityEngine.InputSystem;

public class CursorVisibilityController : MonoBehaviour
{
    
    void FixedUpdate()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        if(Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            Cursor.visible = false;
        }
        else if(Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame)
        {
            Cursor.visible = true;
        }
    }
}
