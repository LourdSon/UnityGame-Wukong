using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceManager : MonoBehaviour
{
    public static string CurrentDeviceType { get; private set; } = "Keyboard";
    public TMPwithIcons[] tMPwithIcons;

    private void OnEnable()
    {
        InputSystem.onActionChange += OnInputActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnInputActionChange;
    }

    private void OnInputActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            var device = ((InputAction)obj).activeControl.device;

            if (device is Gamepad gamepad)
            {
                if (gamepad.name.Contains("DualShock") || gamepad.name.Contains("PlayStation"))
                {
                    CurrentDeviceType = "PlayStation";
                    for(int i = 0; i<tMPwithIcons.Length; i++)
                        tMPwithIcons[i].UpdateTutorialText("playstation");
                }
                else if (gamepad.name.Contains("Xbox") || gamepad.name.Contains("XInput"))
                {
                    CurrentDeviceType = "Xbox";
                    for(int i = 0; i<tMPwithIcons.Length; i++)
                        tMPwithIcons[i].UpdateTutorialText("xbox");
                }
                else
                {
                    CurrentDeviceType = "GenericGamepad";
                    for(int i = 0; i<tMPwithIcons.Length; i++)
                        tMPwithIcons[i].UpdateTutorialText("xbox");
                }
            }
            else if (device is Keyboard)
            {
                CurrentDeviceType = "Keyboard";
                for(int i = 0; i<tMPwithIcons.Length; i++)
                    tMPwithIcons[i].UpdateTutorialText("playstation");
            }
        }
    }
}
