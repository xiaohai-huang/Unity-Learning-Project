using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{

    [SerializeField] private InputReader _inputReader;
    // Start is called before the first frame update
    void Start()
    {
        _inputReader.OnMove += InputReader_OnMove;
        _inputReader.EnableGamePlayActionMap();
    }

    private void InputReader_OnMove(Vector2 arg0)
    {
        print(arg0);
    }
}
