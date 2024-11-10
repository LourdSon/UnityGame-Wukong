using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    public InputActionReference actionToRebind;

    private string rebinds;
    

    void Awake()
    {
        LoadRebinds(); // Charger les rebinds au démarrage du jeu
    }

    public void SaveRebinds()
    {
        rebinds = actionToRebind.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void LoadRebinds()
    {
        rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
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

