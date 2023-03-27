using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] protected InputReader _inputReader;
    private Animator _animator;
    private ClientNetworkAnimator _clientNetworkAnimator;
    private CharacterController _characterController;
    [SerializeField] private Vector2 _movementInput;
    private bool _sprinting;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedChangeRate = 10.0f;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private bool _grounded;
    private Camera _mainCam;

    // Animations
    private readonly int JUMP_ANIMATION_ID = Animator.StringToHash("Jump");

    /// <summary>
    /// Walking speed in m/s
    /// </summary>
    public float WalkSpeed = 1.42f;

    /// <summary>
    /// Running speed in m/s
    /// </summary>
    public float RunningSpeed = 6.7f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _clientNetworkAnimator = GetComponent<ClientNetworkAnimator>();
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        if (!IsOwner) return;
        _inputReader.EnableGamePlayActionMap();
        _inputReader.OnMove += InputReader_MoveEvent;
        _inputReader.OnSprintChanged += InputReader_OnSprintChanged;
        _inputReader.OnJumpStarted += InputReader_OnJumpStarted;
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
            _clientNetworkAnimator.SetTrigger(JUMP_ANIMATION_ID);
            // Prevent the character from moving, because the player is in preparing jump motion
            lockHorizontalMovement = true;
        }
    }

    /// <summary>
    /// Invocked by animation event to add vertical jump speed
    /// </summary>
    public void AddJumpSpeed()
    {
        if (_characterController.isGrounded)
        {
            float jumpHeight = 1.5f;
            _verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

            _clientNetworkAnimator.ResetTrigger(JUMP_ANIMATION_ID);
            lockHorizontalMovement = false;
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

    private void UpdateSpeed()
    {
        float targetSpeed = _sprinting ? RunningSpeed : WalkSpeed;
        if (_movementInput == Vector2.zero) targetSpeed = 0.0f;
        float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;
        float speedTolerance = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedTolerance ||
           currentHorizontalSpeed > targetSpeed + speedTolerance)
        {
            _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * _speedChangeRate);
        }
        else
        {
            _speed = targetSpeed;
        }
    }

    /// <summary>
    /// Rotates the character towards its movement direction based on the camera’s forward vector.
    /// </summary>
    private void RotateCharacter()
    {
        if (_movementInput != Vector2.zero)
        {
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

        _characterController.Move(_speed * Time.deltaTime * transform.forward +
                new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        _animator.SetFloat("Speed", _speed);
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
        _grounded = _characterController.isGrounded;
        _animator.SetBool("Grounded", _grounded);
    }
}

