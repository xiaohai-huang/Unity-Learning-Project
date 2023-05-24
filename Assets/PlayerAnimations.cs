using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimations : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CrossHairTarget _target;
    [SerializeField] private Rig _bodyAimRig;
    [SerializeField] private MultiAimConstraint _headAimConstraint;


    private Animator _animator;
    private ClientNetworkAnimator _clientNetworkAnimator;
    private PlayerController _playerController;


    private readonly int JUMP_ANIMATION_ID = Animator.StringToHash("Jump");
    private readonly int SPEED_ANIMATION_ID = Animator.StringToHash("Speed");
    private readonly int GROUNDED_ANIMATION_ID = Animator.StringToHash("Grounded");



    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        _animator = GetComponent<Animator>();
        _clientNetworkAnimator = GetComponent<ClientNetworkAnimator>();
        _playerController = GetComponent<PlayerController>();
        _playerController.OnAddJumpSpeed += HandleOnAddJumpSpeed;
        _inputReader.OnJumpStarted += InputReader_OnJumpStarted;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.OnJumpStarted -= InputReader_OnJumpStarted;
        _playerController.OnAddJumpSpeed += HandleOnAddJumpSpeed;
    }

    private void InputReader_OnJumpStarted()
    {
        if (_playerController.Grounded)
        {
            _clientNetworkAnimator.SetTrigger(JUMP_ANIMATION_ID);
        }
    }
    private void HandleOnAddJumpSpeed()
    {
        _clientNetworkAnimator.ResetTrigger(JUMP_ANIMATION_ID);
    }

    private void HandleBodyAim()
    {
        // Disable head aim of the player if the user is looking the back of the player while standing.
        Vector3 viewDir = Vector3.ProjectOnPlane(_target.transform.position, Vector3.up).normalized;
        var degrees = Vector3.Angle(transform.forward, viewDir);
        bool isLookingAtBack = degrees > 120;
        if (isLookingAtBack)
        {
            if(degrees > 150)
            {
                _bodyAimRig.weight = Mathf.Lerp(_bodyAimRig.weight, 0, 16 * Time.deltaTime);
            }
            else
            {
                _bodyAimRig.weight = Mathf.Lerp(_bodyAimRig.weight, 0, 8 * Time.deltaTime);
            }
        }
        else
        {
            _bodyAimRig.weight = Mathf.Lerp(_bodyAimRig.weight, 1, 4f * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleBodyAim();
        if (!IsOwner) return;

        _animator.SetFloat(SPEED_ANIMATION_ID, _playerController.Speed);
        _animator.SetBool(GROUNDED_ANIMATION_ID, _playerController.Grounded);
    }
}
