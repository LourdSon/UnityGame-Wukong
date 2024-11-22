using UnityEngine;
using UnityEngine.InputSystem;

public class InputLayout : MonoBehaviour
{
    public GameObject image1;
    public GameObject image2;
    private GameObject pauseMenu;


    void Start()
    {
        pauseMenu = gameObject;

    }

    void Update()
    {
        if(pauseMenu.activeSelf)
        {
            if(Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
            {
                image1.SetActive(true);
                image2.SetActive(false);
            }
            else if(Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame)
            {
                image1.SetActive(false);
                image2.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
