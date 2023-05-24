using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] protected InputReader _inputReader;

    private CharacterController _characterController;
    [SerializeField] private Vector2 _movementInput;
    private bool _sprinting;
    public float Speed { get; private set; }
    [SerializeField] private float _speedChangeRate = 10.0f;
    [SerializeField] private float _verticalSpeed;
    public bool Grounded { get; private set; }
    private Camera _mainCam;


    /// <summary>
    /// Walking speed in m/s
    /// </summary>
    public float WalkSpeed = 1.42f;

    /// <summary>
    /// Running speed in m/s
    /// </summary>
    public float RunningSpeed = 6.7f;

    public bool IsAiming { get; private set; }

    public UnityAction OnAddJumpSpeed;



    void Start()
    {
        if (!IsOwner) return;
        _inputReader.EnableGamePlayActionMap();
        _inputReader.OnMove += InputReader_MoveEvent;
        _inputReader.OnSprintChanged += InputReader_OnSprintChanged;
        _inputReader.OnJumpStarted += InputReader_OnJumpStarted;
        _inputReader.OnAimStartedEvent += InputReader_OnAimStartedEvent;
        _inputReader.OnAimEndEvent += InputReader_OnAimEndEvent;
        _characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _mainCam = Camera.main;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.OnMove -= InputReader_MoveEvent;
        _inputReader.OnSprintChanged -= InputReader_OnSprintChanged;
        _inputReader.OnJumpStarted -= InputReader_OnJumpStarted;
        _inputReader.OnAimStartedEvent -= InputReader_OnAimStartedEvent;
        _inputReader.OnAimEndEvent -= InputReader_OnAimEndEvent;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        base.OnNetworkDespawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        UpdateSpeed();
        RotateCharacter();
        MoveCharacter();
        HandleGravity();
    }


    private bool lockHorizontalMovement;
    private void InputReader_OnJumpStarted()
    {
        if (_characterController.isGrounded)
        {
            // Prevent the character from moving, because the player is in preparing jump motion
            lockHorizontalMovement = true;
        }
    }

    /// <summary>
    /// Invoked by animation event to add vertical jump speed
    /// </summary>
    public void AddJumpSpeed()
    {
        if (!IsOwner) return;
        if (_characterController.isGrounded)
        {
            float jumpHeight = 1.5f;
            _verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

            lockHorizontalMovement = false;
            OnAddJumpSpeed?.Invoke();
        }
    }

    private void InputReader_OnSprintChanged(bool sprinting)
    {
        _sprinting = sprinting;
    }

    private void InputReader_MoveEvent(Vector2 movementInput)
    {
        _movementInput = movementInput;
    }

    private void InputReader_OnAimStartedEvent()
    {
        IsAiming = true;
    }

    private void InputReader_OnAimEndEvent()
    {
        IsAiming = false;
    }


    private void UpdateSpeed()
    {
        float targetSpeed = _sprinting ? RunningSpeed : WalkSpeed;
        if (_movementInput == Vector2.zero) targetSpeed = 0.0f;
        float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;
        float speedTolerance = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedTolerance ||
           currentHorizontalSpeed > targetSpeed + speedTolerance)
        {
            Speed = Mathf.Lerp(Speed, targetSpeed, Time.deltaTime * _speedChangeRate);
        }
        else
        {
            Speed = targetSpeed;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RotateCharacter()
    {
        if (IsAiming)
        {
            // Rotates the player towards camera's forward vector.
            Vector3 camDir = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up).normalized;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(camDir, Vector3.up);
            float rotationSpeed = 10f;

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (_movementInput != Vector2.zero)
        {
            // Rotates the character towards its movement direction based on the camera�s forward vector.

            Vector3 inputDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            // Project the camera's forward vector onto the XZ plane and normalize it
            Vector3 camDir = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up).normalized;

            // Calculate the forward and right vectors relative to the camera
            Vector3 forward = camDir;
            Vector3 right = Vector3.Cross(Vector3.up, forward);

            // Calculate the adjusted direction based on the input and camera vectors
            Vector3 adjustedDir = (forward * inputDir.z + right * inputDir.x).normalized;

            // Calculate the target rotation based on the adjusted direction
            Quaternion targetRotation = Quaternion.LookRotation(adjustedDir, Vector3.up);
            float rotationSpeed = 10f;

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveCharacter()
    {
        if (lockHorizontalMovement) return;

        if (IsAiming)
        {
            Vector3 inputDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            // Project the camera's forward vector onto the XZ plane and normalize it
            Vector3 camDir = Vector3.ProjectOnPlane(_mainCam.transform.forward, Vector3.up).normalized;

            // Calculate the forward and right vectors relative to the camera
            Vector3 forward = camDir;
            Vector3 right = Vector3.Cross(Vector3.up, forward);

            // Calculate the adjusted direction based on the input and camera vectors
            Vector3 adjustedDir = (forward * inputDir.z + right * inputDir.x).normalized;

            _characterController.Move(Speed * Time.deltaTime * adjustedDir +
                        new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        }
        else

        {
            _characterController.Move(Speed * Time.deltaTime * transform.forward +
                new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        }

    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            _verticalSpeed = Physics.gravity.y;
        }
        else
        {
            _verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }
        Grounded = _characterController.isGrounded;
    }

}

