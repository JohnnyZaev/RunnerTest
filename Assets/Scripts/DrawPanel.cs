using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DrawPanel : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField][Range(0f, 2f)] private float lineWidth = 0.2f;
    [SerializeField] private CharactersAligner charactersAligner;
    private RectTransform _transform;
    private Camera _mainCamera;
    private List<Vector3> _lineScreenPositions = new ();

    private Vector3 _previousPosition;
    private bool _reseted;
    private Vector3[] _positions;

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
        _lineScreenPositions.Clear();
        _reseted = true;
    }

    private void Update()
    {
        if (Touch.activeTouches.Count != 1 || !RectTransformUtility.RectangleContainsScreenPoint(_transform, Touch.activeTouches[0].screenPosition))
        {
            if (!_reseted)
            {
                // _positions = new Vector3[lineRenderer.positionCount];
                // lineRenderer.GetPositions(_positions);
                charactersAligner.RealignCharacters(_lineScreenPositions);
                ResetLine();
            }
            return;
        }

        _reseted = false;
        Vector3 touchPos = Touch.activeTouches[0].screenPosition;
        touchPos.z = 10;
        Vector3 currentPosition = _mainCamera.ScreenToWorldPoint(touchPos);
        // currentPosition.z = 0;

        if (Vector3.Distance(currentPosition, _previousPosition) > minDistance)
        {
            if (_lineScreenPositions.Count < 1)
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
            _lineScreenPositions.Add(touchPos);
        }

        for (int i = 0; i < _lineScreenPositions.Count; i++)
        {
            lineRenderer.SetPosition(i, _mainCamera.ScreenToWorldPoint(_lineScreenPositions[i]));
        }
    }
}
