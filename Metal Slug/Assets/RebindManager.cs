using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    public InputActionReference actionToRebind;

    void Awake()
    {
        LoadRebinds(); // Charger les rebinds au démarrage du jeu
    }

    public void SaveRebinds()
    {
        string rebinds = actionToRebind.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void LoadRebinds()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionToRebind.action.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void OnRebindCompleted()
    {
        SaveRebinds(); // Sauvegarde des rebinds lorsque la réattribution des touches est terminée
    }
}

