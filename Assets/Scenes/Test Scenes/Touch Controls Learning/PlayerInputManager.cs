using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]private InputReader _inputReader;
    public GameObject Player;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        _inputReader.EnableGamePlayActionMap();
        _inputReader.OnLookEvent += InputReader_OnLookEvent;
    }

    private void InputReader_OnLookEvent(Vector2 arg0)
    {
        print("look event data: " + arg0);
        cam.transform.Translate(arg0);
    }
}
