using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HoldWeapon : MonoBehaviour
{
    [SerializeField] private Transform _leftHandIKTarget;
    [SerializeField] private Transform _rightHandIKTarget;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rig _gunIdleRig;
    [SerializeField] private Rig _gunAimingRig;
    public Weapon ActiveWeapon;
    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _inputReader.OnAttackStartedEvent += InputReader_OnAttackStartedEvent;
        _inputReader.OnAttackEndEvent += InputReader_OnAttackEndEvent;
     
        _playerController = GetComponent<PlayerController>();
    }



    private void InputReader_OnAttackEndEvent()
    {
        ActiveWeapon.StopFire();
    }

    private void InputReader_OnAttackStartedEvent()
    {
        if (_playerController.IsAiming)
        {
            ActiveWeapon.StartFire();
        }
    }

    float _transitionSpeed = 10f;
    void Update()
    {
        if (_playerController.IsAiming)
        {
            _gunAimingRig.weight = Mathf.Lerp(_gunAimingRig.weight, 1, _transitionSpeed * Time.deltaTime);
            _gunIdleRig.weight = Mathf.Lerp(_gunIdleRig.weight, 0, _transitionSpeed * Time.deltaTime);
        }
        else
        {
            _gunAimingRig.weight = Mathf.Lerp(_gunAimingRig.weight, 0, _transitionSpeed * Time.deltaTime);
            _gunIdleRig.weight = Mathf.Lerp(_gunIdleRig.weight, 1, _transitionSpeed * Time.deltaTime);
            ActiveWeapon.StopFire();
        }

        _leftHandIKTarget.SetPositionAndRotation(ActiveWeapon.LeftHandGrip.position, ActiveWeapon.LeftHandGrip.transform.rotation);
        _rightHandIKTarget.SetPositionAndRotation(ActiveWeapon.RightHandGrip.position, ActiveWeapon.RightHandGrip.transform.rotation);
    }
}
