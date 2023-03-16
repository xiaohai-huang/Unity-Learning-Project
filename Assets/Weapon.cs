using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform LeftHandGrip;
    public Transform RightHandGrip;
    public float ForceStrength = 30f;
    private bool _firing;
    public int BulletsPerSecond = 10;
    private float _secondsPerBullet;

    public bool Firing => _firing;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform _raycastDestination;
    [SerializeField] private GameObject _hitEffect;


    private Ray ray;
    public void StartFire()
    {
        _firing = true;

    }

    public void StopFire()
    {
        _firing = false;

    }

    private void Start()
    {
        _secondsPerBullet = 1.0f / BulletsPerSecond;
    }
    private float _timeSinceLastFire;
    private float _lastFireTime;

    private void Update()
    {
        if (Firing)
        {
            _timeSinceLastFire = Time.time - _lastFireTime;
            if (_timeSinceLastFire > _secondsPerBullet)
            {
                _muzzleFlash.Emit(1);
                ray.origin = _raycastOrigin.position;
                ray.direction = (_raycastDestination.position - _raycastOrigin.position).normalized;
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    var effect = Instantiate(_hitEffect);
                    effect.transform.position = ray.origin;
                    effect.transform.forward = ray.direction;
                    effect.SetActive(true);
                    effect.GetComponent<Rigidbody>().AddForce(ray.direction * ForceStrength, ForceMode.Impulse);
                }
                _lastFireTime = Time.time;
            }
        }
    }
}
