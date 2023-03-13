using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Input Reader")]
public class InputReader : ScriptableObject, MyGameInputActions.IGamePlayActionMapActions
{
    public event UnityAction<Vector2> OnMove;
    public event UnityAction<bool> OnSprintChanged;
    public event UnityAction OnJumpStarted;
    public event UnityAction<Vector2> OnLookEvent;
    

    private MyGameInputActions _inputActions;

  

    private void OnEnable()
    {
        if(_inputActions == null)
        {
            _inputActions = new MyGameInputActions();

            _inputActions.GamePlayActionMap.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        // Disable all action maps
        _inputActions.GamePlayActionMap.Disable();
    }

    public void EnableGamePlayActionMap()
    {
        _inputActions.GamePlayActionMap.Enable();
        // disable others
    }



    public void OnMoveAction(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Performed:
                {
                    OnSprintChanged?.Invoke(true);
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    OnSprintChanged?.Invoke(false);
                    break;
                }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Performed)
        {
            OnJumpStarted?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        OnLookEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
