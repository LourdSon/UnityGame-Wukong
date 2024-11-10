using UnityEngine;
using UnityEngine.InputSystem;

public class LoadBindings : MonoBehaviour
{
    public InputActionAsset inputActions;
    private const string RebindsKey = "inputRebinds";

    public void Load()
    {
        if (PlayerPrefs.HasKey(RebindsKey))
        {
            // Charge les bindings depuis les PlayerPrefs en JSON
            string rebinds = PlayerPrefs.GetString(RebindsKey);
            
            inputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    private void Start()
    {
        Load();
    }
}
