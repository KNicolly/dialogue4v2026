using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    [Header("Movement")]
    [Tooltip("Acceleration applied to the rigidbody when input is received (units/s^2)")]
    public float moveAcceleration = 10f;

    [Tooltip("Maximum horizontal speed (m/s). Set to <= 0 to disable clamping.")]
    public float maxSpeed = 6f;

    private Rigidbody m_Rigidbody;
    private Vector2 m_MoveInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        m_Rigidbody = GetComponent<Rigidbody>();

        if (m_Rigidbody == null)
            Debug.LogError("PlayerController requires a Rigidbody on the same GameObject.");
    }

    void Start()
    {
        StartCoroutine(CarregarCheckpointDepois());
    }

    private IEnumerator CarregarCheckpointDepois()
    {
        yield return null;

        CarregarCheckpoint();
    }

    void OnEnable()
    {
        playerInput.actions.FindAction("Move").performed += OnMovePerformed;
        playerInput.actions.FindAction("Move").canceled += OnMoveCanceled;

        playerInput.actions.FindAction("Interact").performed += OnInteract;
    }

    void OnDisable()
    {
        playerInput.actions.FindAction("Move").performed -= OnMovePerformed;
        playerInput.actions.FindAction("Move").canceled -= OnMoveCanceled;

        playerInput.actions.FindAction("Interact").performed -= OnInteract;
    }

    void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        m_MoveInput = ctx.ReadValue<Vector2>();
    }

    void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        m_MoveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (m_Rigidbody == null)
            return;

        Vector3 desired = new Vector3(
            m_MoveInput.x,
            0f,
            m_MoveInput.y
        );

        if (desired.sqrMagnitude > 0f)
        {
            Vector3 accel =
                desired.normalized * moveAcceleration;

            m_Rigidbody.AddForce(
                accel,
                ForceMode.Acceleration
            );
        }

        if (maxSpeed > 0f)
        {
            Vector3 horizontalVel =
                new Vector3(
                    m_Rigidbody.linearVelocity.x,
                    0f,
                    m_Rigidbody.linearVelocity.z
                );

            float speed = horizontalVel.magnitude;

            if (speed > maxSpeed)
            {
                Vector3 limited =
                    horizontalVel.normalized * maxSpeed;

                m_Rigidbody.linearVelocity =
                    new Vector3(
                        limited.x,
                        m_Rigidbody.linearVelocity.y,
                        limited.z
                    );
            }
        }
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        InteractOM.Interact();
    }

    private void CarregarCheckpoint()
    {
        if (SaveSystem.Instance == null)
            return;

        SaveSystem.Save save =
            SaveSystem.Instance.GetSave(0);

        if (save == null)
            return;

        if (!save.checkpointAtivado)
            return;

        string cenaAtual =
            SceneManager.GetActiveScene().name;

        // Verifica se o checkpoint pertence à fase atual
        if (save.fase == 1 && cenaAtual != "fase1")
            return;

        if (save.fase == 2 && cenaAtual != "fase2")
            return;

        transform.position = new Vector3(
            save.checkpointX,
            save.checkpointY,
            save.checkpointZ
        );

        m_Rigidbody.linearVelocity = Vector3.zero;

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.LoadCoins(
                save.moedasNoCheckpoint,
                save.moedasColetadas
            );
        }

        Debug.Log(
            "Checkpoint da fase " +
            save.fase +
            " carregado!"
        );
    }
}