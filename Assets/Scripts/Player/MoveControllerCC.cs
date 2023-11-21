using UnityEngine;

namespace ProjectZee
{
    [RequireComponent(typeof(CharacterController))]
    public class MoveControllerCC : MonoBehaviour
    {
        #region Variables

        [Header("Movement")]
        [SerializeField] private CharacterController cc;
        [SerializeField][Range(1f, 10f)] private float moveSpeed = 3.25f;

        // ----------------------------------------------------------------------

        [Header("Jump")]
        [SerializeField] private float gravity = -18f;
        [SerializeField][Range(1f, 5f)] private float jumpHeight = 1.25f;
        [SerializeField][Range(0.1f, 1f)] private float groundDistance = 0.4f;

        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;

        private bool isGrounded;
        private Vector3 velocity;

        // ----------------------------------------------------------------------

        [Header("Camera")]
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Vector2 minMaxLook = new(-60f, 65f);
        [SerializeField][Range(1f, 10f)] private float mouseSensitivity = 2f;

        private float verticalRotation = 0f;

        #endregion

        // ----------------------------------------------------------------------

        #region Main Methods

        private void Start()
        {
            if (!cc) cc = GetComponent<CharacterController>();
            if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            PlayerController();
            CameraLook();
        }

        #endregion

        // ----------------------------------------------------------------------

        #region Custom Methods

        /// <summary>
        /// Main player controller method responsible for coordinating movement and jumping.
        /// </summary>
        private void PlayerController()
        {
            MoveController();
            JumpController();
        }

        /// <summary>
        /// Handles player movement based on user input.
        /// </summary>
        private void MoveController()
        {
            // Retrieve input for vertical and horizontal movement.
            float verticalMovement = Input.GetAxisRaw("Vertical");
            float horizontalMovement = Input.GetAxisRaw("Horizontal");

            // Calculate the movement vector based on input.
            Vector3 movement = transform.right * horizontalMovement + transform.forward * verticalMovement;

            // Move the character controller based on the calculated movement vector.
            cc.Move(moveSpeed * Time.deltaTime * movement.normalized);
        }

        /// <summary>
        /// Manages player jumping behavior, considering gravity and ground detection.
        /// </summary>
        private void JumpController()
        {
            // Check if the player is grounded using a sphere cast.
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            // Apply a small negative velocity when grounded to ensure better grounding.
            if (isGrounded && velocity.y < 0) velocity.y = -2f;

            // Check for jump input and apply vertical velocity accordingly
            if (isGrounded && Input.GetKeyDown(jumpKey)) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Apply gravity to the vertical velocity.
            velocity.y += gravity * Time.deltaTime;

            // Move the character controller based on the final velocity.
            cc.Move(velocity * Time.deltaTime);
        }

        /// <summary>
        /// Handles camera rotation based on mouse input.
        /// </summary>
        private void CameraLook()
        {
            // Retrieve mouse input for X and Y axis.
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Adjust the vertical rotation based on mouse Y input, clamped within specified limits.
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, minMaxLook.x, minMaxLook.y);

            // Apply the vertical rotation to the camera holder's local rotation.
            cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

            // Rotate the player character around the Y axis based on mouse X input.
            transform.Rotate(Vector3.up * mouseX);
        }
        #endregion

        // ----------------------------------------------------------------------
    }
}
