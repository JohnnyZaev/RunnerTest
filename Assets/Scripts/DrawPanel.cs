using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DrawPanel : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField][Range(0f, 2f)] private float lineWidth = 0.2f;
    private RectTransform _transform;
    private Camera _mainCamera;

    private Vector3 _previousPosition;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _mainCamera = Camera.main;

        ResetLine();
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
    }

    private void ResetLine()
    {
        lineRenderer.positionCount = 1;
        var transform1 = lineRenderer.transform;
        _previousPosition = transform1.position;
    }

    private void Update()
    {
        if (Touch.activeTouches.Count != 1 || !RectTransformUtility.RectangleContainsScreenPoint(_transform, Touch.activeTouches[0].screenPosition))
        {
            ResetLine();
            return;
        }

        Vector3 touchPos = Touch.activeTouches[0].screenPosition;
        touchPos.z = 10;
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(touchPos);
        // currentPosition.z = 0;

        if (Vector3.Distance(currentPosition, _previousPosition) > minDistance)
        {
            if (_previousPosition == lineRenderer.transform.position)
            {
                lineRenderer.SetPosition(0, currentPosition);
            }
            else
            {
                var positionCount = lineRenderer.positionCount;
                positionCount++;
                lineRenderer.positionCount = positionCount;
                lineRenderer.SetPosition(positionCount - 1, currentPosition);
            }
            
            _previousPosition = currentPosition;
        }
        Debug.Log($"Touch in area {currentPosition}");
    }
}
