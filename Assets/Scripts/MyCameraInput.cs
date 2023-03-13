using Cinemachine;
using UnityEngine;

public class MyCameraInput : MonoBehaviour
{
    public Transform CameraFollowTarget;


    public AxisState X_Axis;
    public AxisState Y_Axis;
    void Start()
    {
        var _provider = GetComponent<AxisState.IInputAxisProvider>();
        X_Axis.SetInputAxisProvider(0,_provider);
        Y_Axis.SetInputAxisProvider(1, _provider);

        // mobile

    }
    private void Update()
    {
        X_Axis.Update(Time.deltaTime);
        Y_Axis.Update(Time.deltaTime);
    }


    private void LateUpdate()
    {
        CameraFollowTarget.rotation = Quaternion.Euler(Y_Axis.Value, X_Axis.Value, 0);
    }
}
