using Unity.Netcode;
using UnityEngine;

public class NetworkAnimatorTest : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private Animator _animator;
    private ClientNetworkAnimator _clientNetworkAnimator;

    private void Awake()
    {
        _clientNetworkAnimator = GetComponent<ClientNetworkAnimator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        _inputReader.EnableGamePlayActionMap();
        _inputReader.OnJumpStarted += _inputReader_OnJumpStarted;
    }

    private void _inputReader_OnJumpStarted()
    {
        _clientNetworkAnimator.SetTrigger("Jump");
        print("jump");
    }
    public void CleanUpTrigger()
    {
        _clientNetworkAnimator.ResetTrigger("Jump");
        print("reset trigger jump");
    }
}
