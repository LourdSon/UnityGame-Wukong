using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; // Texte du tutoriel
    public string[] tutorialSteps; // Étapes du tutoriel
    private int currentStep = 0; // Étape actuelle
    public TutorialScript tutorialScript;
    public PlayerShooting playerShooting;
    public PlayerAttack playerAttack;
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public int firstTime;

    private void Start()
    {
        if (tutorialSteps.Length > 0)
        {
            tutorialScript.isInTutorialText = true;
            UpdateTutorialText();
        }
        else
        {
            Debug.LogWarning("Aucune étape de tutoriel n'a été configurée !");
        }
    }

    private void Update()
    {
        if (IsSpecialStep())
        {
            // Les étapes spéciales permettent uniquement l'action attendue
            tutorialScript.isInTutorialText = false;
            VerifyTutorialStep();
        }
        else if (tutorialScript.isInTutorialText && PlayerController.instance.playerInputActions.Player.Validate.triggered)
        {
            // Valide les étapes normales
            NextStep();
        }
    }

    private bool IsSpecialStep()
    {
        // Étapes nécessitant une action spécifique
        return currentStep == 2 || currentStep == 3 || currentStep == 6 || currentStep == 8 ||
               currentStep == 9 || currentStep == 13 || currentStep == 15 || currentStep == 17 ||
               currentStep == 20 || currentStep == 23 || currentStep == 26;
    }

    private void VerifyTutorialStep()
    {
        switch (currentStep)
        {
            case 2:
                CheckMoveInput();
                break;
            case 3:
                CheckJumpInput();
                break;
            case 6:
                CheckAttackInput(Vector2.zero);
                break;
            case 8:
                CheckAttackInput(Vector2.up);
                break;
            case 9:
                CheckAttackInput(Vector2.down);
                break;
            case 13:
                CheckChargingKi();
                break;
            case 15:
                CheckShootInput();
                break;
            case 17:
                CheckSuperShotInput();
                break;
            case 20:
                CheckSpecialAttacks();
                break;
            case 23:
                CheckHealing();
                break;
            case 26:
                CheckDashInput();
                break;
        }
    }

    private void CheckMoveInput()
    {
        // tutorialScript.isInTutorialText = false;
        if (PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            CompleteStep();
        }
    }

    private void CheckJumpInput()
    {
        if (PlayerController.instance.playerInputActions.Player.Jump.triggered)
        {
            CompleteStep();
        }
    }

    private void CheckAttackInput(Vector2 direction)
    {
        if (PlayerController.instance.playerInputActions.Player.Attack.triggered &&
            PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>() == direction)
        {
            CompleteStep();
        }
    }

    private void CheckChargingKi()
    {
        playerMovement.currentKi = 0;
        playerMovement.UpdateKiBar();
        if (playerMovement.isCharging)
        {
            CompleteStep();
        }
    }

    private void CheckShootInput()
    {
        if (PlayerController.instance.playerInputActions.Player.Shoot.triggered)
        {
            CompleteStep();
        }
    }

    private void CheckSuperShotInput()
    {
        playerMovement.currentKi = playerMovement.maxKi;
        playerMovement.UpdateKiBar();
        if (PlayerController.instance.playerInputActions.Player.SuperShot.triggered &&
            PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            CompleteStep();
        }
    }

    private void CheckSpecialAttacks()
    {
        if (playerShooting.isShooting4 || playerAttack.attack6 || playerAttack.attack7)
        {
            CompleteStep();
        }
    }

    private void CheckHealing()
    {
        playerHealth.health = playerHealth.maxHealth / 2;
        playerHealth.UpdateHealthBar();
        if (playerHealth.isHealing)
        {
            CompleteStep();
        }
    }

    private void CheckDashInput()
    {
        if (PlayerController.instance.playerInputActions.Player.Dash.triggered &&
            PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            CompleteStep();
        }
    }

    private void CompleteStep()
    {
        // Étapes spéciales validées
        StartCoroutine(ReEnableTutorialText()); // Rebloque les actions après la validation
        NextStep();
    }

    private void NextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            UpdateTutorialText();
        }
        else
        {
            EndTutorial();
        }
    }

    private void UpdateTutorialText()
    {
        tutorialText.text = tutorialSteps[currentStep];
    }

    private void EndTutorial()
    {
        enabled = false;
        int nextSceneIndex = PlayerPrefs.HasKey("firstTime") ? SceneManager.GetActiveScene().buildIndex - 1 : SceneManager.GetActiveScene().buildIndex - 2;
        firstTime = 1;
        PlayerPrefs.SetInt("firstTime", firstTime);
        Physics2D.gravity = new Vector2(0, -9.81f);
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator ReEnableTutorialText()
    {
        yield return new WaitForSeconds(0.5f);
        tutorialScript.isInTutorialText = true;
        
    }
}
