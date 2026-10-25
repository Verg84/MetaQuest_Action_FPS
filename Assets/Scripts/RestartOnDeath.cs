using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class RestartOnDeath : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private InputAction restartAction;
    private bool restarting;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();

        restartAction = new InputAction(
            name: "Restart",
            type: InputActionType.Button,
            binding: "<XRController>{LeftHand}/secondaryButton"
        );
    }

    private void OnEnable()
    {
        restartAction.Enable();
    }

    private void OnDisable()
    {
        restartAction.Disable();
    }

    private void Update()
    {
        if (restarting || !playerHealth.IsDead)
            return;

        if (restartAction.WasPressedThisFrame())
        {
            restarting = true;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }
    }

    private void OnDestroy()
    {
        restartAction?.Dispose();
    }
}