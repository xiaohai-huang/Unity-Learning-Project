using Unity.Netcode;
using UnityEngine;

public class PlayerAnimations : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;

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
    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat(SPEED_ANIMATION_ID, _playerController.Speed);
        _animator.SetBool(GROUNDED_ANIMATION_ID, _playerController.Grounded);
    }
}
