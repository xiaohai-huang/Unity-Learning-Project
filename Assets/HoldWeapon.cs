using UnityEngine;

public class HoldWeapon : MonoBehaviour
{
    [SerializeField] private Transform _leftHandIKTarget;
    [SerializeField] private Transform _rightHandIKTarget;
    [SerializeField] private InputReader _inputReader;
    public Weapon ActiveWeapon;
    // Start is called before the first frame update
    void Start()
    {
        _inputReader.OnAttackStartedEvent += InputReader_OnAttackStartedEvent;
        _inputReader.OnAttackEndEvent += InputReader_OnAttackEndEvent;
    }

    private void InputReader_OnAttackEndEvent()
    {
        ActiveWeapon.StopFire();
    }

    private void InputReader_OnAttackStartedEvent()
    {
        ActiveWeapon.StartFire();
    }

    // Update is called once per frame
    void Update()
    {
        _leftHandIKTarget.SetPositionAndRotation(ActiveWeapon.LeftHandGrip.position, ActiveWeapon.LeftHandGrip.transform.rotation);
        _rightHandIKTarget.SetPositionAndRotation(ActiveWeapon.RightHandGrip.position, ActiveWeapon.RightHandGrip.transform.rotation);
    }
}
