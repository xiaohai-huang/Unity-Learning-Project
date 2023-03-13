using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenViewPanel : OnScreenControl, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;
    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    private Vector2 _position;
    private bool _shouldSendData;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _shouldSendData = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _position = eventData.position;
        SendValueToControl(eventData.delta);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        SendValueToControl(Vector2.zero);
        _shouldSendData = false;
    }

    private Vector2 _prevPosition;
    private void Update()
    {
        if(_shouldSendData)
        {
            if (_prevPosition == _position) SendValueToControl(Vector2.zero);
            _prevPosition = _position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = ((RectTransform)transform).localToWorldMatrix;
        var rect = ((RectTransform)transform).rect;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(rect.min, new Vector2(rect.max.x, rect.min.y));
        Gizmos.DrawLine(rect.max, new Vector2(rect.min.x, rect.max.y));

        Gizmos.DrawLine(new Vector2(rect.max.x, rect.min.y), rect.max);
        Gizmos.DrawLine(rect.min, new Vector2(rect.min.x, rect.max.y));
        Gizmos.matrix = Matrix4x4.identity;
    }


}
