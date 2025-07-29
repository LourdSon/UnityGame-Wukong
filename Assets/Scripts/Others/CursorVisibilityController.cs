using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    public float idleTime = 2f; // Temps d'inactivité avant de cacher la souris
    private float lastMouseMoveTime;
    public GameObject pauseMenu;

    void FixedUpdate()
    {
        if(!pauseMenu.activeSelf)
        {
            // Détecter un mouvement de la souris
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                lastMouseMoveTime = Time.time;
                ShowCursor();
            }

            // Cacher la souris après une période d'inactivité
            if (Time.time - lastMouseMoveTime > idleTime)
            {
                HideCursor();
            }
        } else if(pauseMenu.activeSelf)
        {
            ShowCursor();
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // Évite que la souris sorte de l'écran
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Permet de déplacer librement la souris
    }
}
