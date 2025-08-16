using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_speed = 5f; // Movement speed
    public bool m_IsDirectControl;
    public bool m_isInteracting;  //if a player is solving a puzzle, they cannot move.

    [Header("Audio")]
    public AudioSource m_MovementAudio; // Reference to the audio source used to play person idling sounds
    public AudioClip m_MovementIdle; //player idle noise, if implemented.
    public AudioClip m_MovementSteps; //player footsteps/robot noise when moving.

    
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Set idle sound at start
        if (m_MovementAudio != null && m_MovementIdle != null)
        {
            m_MovementAudio.clip = m_MovementIdle;
            m_MovementAudio.loop = true;
            m_MovementAudio.Play();
        }
    }

    void Update()
    {
        if (!m_IsDirectControl || m_isInteracting) return;

        // Get input
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S

        // Movement vector
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        controller.Move(move * m_speed * Time.deltaTime);

        // --- AUDIO LOGIC ---
        bool isMoving = move.magnitude > 0.1f;

        if (isMoving && m_MovementAudio.clip != m_MovementSteps)
        {
            // Switch to walking sound
            m_MovementAudio.clip = m_MovementSteps;
            m_MovementAudio.loop = true;
            m_MovementAudio.Play();
        }
        else if (!isMoving && m_MovementAudio.clip != m_MovementIdle)
        {
            // Switch back to idle sound
            m_MovementAudio.clip = m_MovementIdle;
            m_MovementAudio.loop = true;
            m_MovementAudio.Play();
        }
    }
}
