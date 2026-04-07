using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel = null;

    private readonly float restartInputHoldTime = 0.25f;

    private float currentRestartInputHoldTime = 0.0f;

    private void Start()
    {
        // Register to OnGatePassed event.
        Gate.OnPassed += Gate_OnPassed;
    }

    private void Gate_OnPassed(Gate gate)
    {
        // First gate passed, disable tutorial.
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

        // Unregister itself.
        Gate.OnPassed -= Gate_OnPassed;
    }

    private void Update()
    {
        // Restart ?
        KeyControl restartKey = Keyboard.current.rKey;

        if (restartKey.isPressed)
        {
            currentRestartInputHoldTime += Time.deltaTime;
        }
        else
        {
            currentRestartInputHoldTime = 0.0f;
        }

        if (currentRestartInputHoldTime >= restartInputHoldTime)
        {
            // Restart //

            Scene currentScene = gameObject.scene;
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
