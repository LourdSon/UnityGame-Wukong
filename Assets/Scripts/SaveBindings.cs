using UnityEngine;
using UnityEngine.InputSystem;

public class SaveBindings : MonoBehaviour
{
    public InputActionAsset inputActions;
    private const string RebindsKey = "inputRebinds";

    public void Save()
    {
        string rebinds = inputActions.ToJson();
        PlayerPrefs.SetString(RebindsKey, rebinds);
        PlayerPrefs.Save();
    }
}
