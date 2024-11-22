using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIInputIconUpdater : MonoBehaviour
{
    public Image jumpButtonIcon;
    public Sprite keyboardIcon;
    public Sprite xboxIcon;
    public Sprite playStationIcon;

    // private string lastDeviceType;
    public TMPwithIcons[] tMPwithIcons;
    private string lastDeviceType; 

    void Update()
    {
        Detect();
    }

    public void Detect()
    {

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {

            if (Gamepad.current.name.Contains("Xbox") || Gamepad.current.name.Contains("XInput"))
            {
                lastDeviceType = "Xbox";
                UpdateIcon(lastDeviceType);
                for(int i = 0; i < tMPwithIcons.Length; i++)
                    tMPwithIcons[i].UpdateTutorialText("xbox");
                return;
            }
            else if (Gamepad.current.name.Contains("Dualshock") || Gamepad.current.name.Contains("Playstation"))
            {
                lastDeviceType = "PlayStation";
                UpdateIcon(lastDeviceType);
                for(int i = 0; i < tMPwithIcons.Length; i++)
                    tMPwithIcons[i].UpdateTutorialText("playstation");
                return;
            }
        }

        if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame)
        {
            lastDeviceType = "Keyboard";
            UpdateIcon(lastDeviceType);
            for(int i = 0; i < tMPwithIcons.Length; i++)
                    tMPwithIcons[i].UpdateTutorialText("keyboard");
            return;
        }
    }





    private void UpdateIcon(string deviceType)
    {
        switch (deviceType)
        {
            case "Keyboard":
                Debug.Log("Keyboard detected");
                if (jumpButtonIcon != null)
                    jumpButtonIcon.sprite = keyboardIcon;
                
                // UpdateTutorialTexts("keyboard");
                break;
            case "Xbox":
                Debug.Log($"Gamepad detected: {Gamepad.current.name}");
                if (jumpButtonIcon != null)
                    jumpButtonIcon.sprite = xboxIcon;
                
                // UpdateTutorialTexts("xbox");
                break;
            case "PlayStation":
                Debug.Log("Playstation detected");
                if (jumpButtonIcon != null)
                    jumpButtonIcon.sprite = playStationIcon;
                
                // UpdateTutorialTexts("playstation");
                break;
        }
    }

    private void UpdateTutorialTexts(string deviceType)
    {
        foreach (var tmpIcon in tMPwithIcons)
        {
            tmpIcon.UpdateTutorialText(deviceType);
        }
    }
}
