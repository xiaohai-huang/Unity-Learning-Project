using UnityEngine;

public class CameraInputProvider : MonoBehaviour, Cinemachine.AxisState.IInputAxisProvider
{
    [SerializeField] private InputReader _inputReader;
    public Vector2 XY_Axis;
    public float GetAxisValue(int axis)
    {
        return axis switch
        {
            0 => XY_Axis.x,
            1 => XY_Axis.y,
            2 => 0,
            _ => 0,
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputReader.OnLookEvent += InputReader_OnLookEvent;
    }

    private void InputReader_OnLookEvent(Vector2 XY_Axis)
    {
        this.XY_Axis = XY_Axis;
    }
}
