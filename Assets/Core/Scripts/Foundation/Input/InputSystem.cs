using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private bool multiTouchEnabled = true;

    private Camera mainCamera;

    private Vector3 previousMousePosition;
    private Vector3 currentMousePosition;
    private Vector3 deltaMousePosition;

    private Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }

    private bool IsCameraIncluded => MainCamera != null;

    public event Action OnTouch;
    public event Action OnRelease;
    public event Action<Vector2> OnDragAction;

    private void Start()
    {
        mainCamera = Camera.main;
        Input.multiTouchEnabled = multiTouchEnabled;
    }

    public Vector2 GetNormalizedDragInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            currentMousePosition = Input.mousePosition;
            deltaMousePosition = currentMousePosition - previousMousePosition;
            previousMousePosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            previousMousePosition = Vector3.zero;
            currentMousePosition = Vector3.zero;
            deltaMousePosition = Vector3.zero;
        }

        return new Vector2(deltaMousePosition.x / Screen.width, deltaMousePosition.y / Screen.height);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsCameraIncluded)
        {
            OnTouch?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsCameraIncluded)
        {
            OnRelease?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsCameraIncluded)
        {
            OnDragAction?.Invoke(new Vector2(eventData.delta.x / Screen.width, eventData.delta.y / Screen.height));
        }
    }
}