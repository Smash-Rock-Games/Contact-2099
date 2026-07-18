using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StoneSmashGames.Contact.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Variables")]
        public bool canMove = true;
        public float speed = 6;
        public Vector3 velocity;

        [Header("Camera Variables")]
        public bool canLook = true;
        public Transform cameraHandle;
        public Transform cameraObject;
        public float inputSensitivity = 0.3f;

        [Header("Movement Input Variables")]
        [SerializeField]
        private float movementInputSmoothing = 0.1f;
        public Vector2 targetMovementInputDelta = Vector2.zero;
        public Vector2 currentMovementInputDelta = Vector2.zero;
        Vector2 movementInputDeltaVelocity;
        float gravityForce = 0;

        [Header("Camera Input Variables")]
        [SerializeField]
        private float cameraInputSmoothing = 0.01f;
        public Vector2 targetCameraInputDelta = Vector2.zero;
        public Vector2 currentCameraInputDelta = Vector2.zero;
        public float cameraPitch = 0f;
        Vector2 cameraInputDeltaVelocity;

        CharacterController characterController;
        bool isCursorLocked;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            LockReleaseCursor(true);
        }

        
        void Update()
        {
            PlayerGravity();
            PlayerMovement();
            PlayerCamera();
        }

        public void LockReleaseCursor(bool _isCursorLocked)
        {
            isCursorLocked = _isCursorLocked;

            if (isCursorLocked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        void PlayerGravity()
        {
            if (!characterController.isGrounded)
                gravityForce += -14 * Time.deltaTime;
            else
                gravityForce = -1;

            Vector3 _gravityVelocity = transform.up * gravityForce;
            characterController.Move(_gravityVelocity * Time.deltaTime);
        }

        void PlayerMovement()
        {
            if (!canMove) { return; }

            targetMovementInputDelta = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetMovementInputDelta.Normalize();

            currentMovementInputDelta = Vector2.SmoothDamp(currentMovementInputDelta, targetMovementInputDelta, ref movementInputDeltaVelocity, movementInputSmoothing);

            velocity = (transform.forward * currentMovementInputDelta.y + transform.right * currentMovementInputDelta.x) * speed;
            characterController.Move(velocity * Time.deltaTime);
        }

        void PlayerCamera()
        {
            if (!canLook) { return; }

            targetCameraInputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            currentCameraInputDelta = Vector2.SmoothDamp(currentCameraInputDelta, targetCameraInputDelta, ref cameraInputDeltaVelocity, movementInputSmoothing);

            cameraPitch -= targetCameraInputDelta.y * inputSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);

            cameraHandle.localEulerAngles = Vector3.right * cameraPitch;
            cameraObject.localEulerAngles = Vector3.forward * -currentMovementInputDelta.x * 2;
            transform.Rotate(Vector3.up * currentCameraInputDelta.x * inputSensitivity);
        }
    }
}
