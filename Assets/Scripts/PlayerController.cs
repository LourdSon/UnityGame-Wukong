using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;  // Singleton pour faciliter l'accès

    public PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Pour garder l'input manager actif à travers toutes les scènes
        }
        else
        {
            //Destroy(gameObject);
        }

        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
}