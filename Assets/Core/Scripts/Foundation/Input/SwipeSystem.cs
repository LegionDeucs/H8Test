using System;
using UnityEngine;
using Zenject;

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft
}

public class SwipeSystem : MonoBehaviour
{
    private class GetCardinalDirections
    {
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);

        public static readonly Vector2 UpRight = new Vector2(1, 1);
        public static readonly Vector2 UpLeft = new Vector2(-1, 1);
        public static readonly Vector2 DownRight = new Vector2(1, -1);
        public static readonly Vector2 DownLeft = new Vector2(-1, -1);
    }

    [Inject] private InputSystem _inputSystem;

    private Vector3 _startTouchPosition;
    private Vector3 _endTouchPosition;

    public event Action<SwipeData> OnFourSwipe = delegate { };
    public event Action<SwipeData> OnEightSwipe = delegate { };
    public event Action<SwipeData> OnFourXTypeSwipe = delegate { };

    private void OnEnable()
    {
        _inputSystem.OnTouch += OnTouch;
        _inputSystem.OnRelease += OnRelease;
    }

    private void OnDisable()
    {
        _inputSystem.OnTouch -= OnTouch;
        _inputSystem.OnRelease -= OnRelease;
    }

    private void OnTouch()
    {
        _startTouchPosition = Input.mousePosition;
    }

    private void OnRelease()
    {
        _endTouchPosition = Input.mousePosition;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        var currentSwipe = new Vector3(_endTouchPosition.x - _startTouchPosition.x, _endTouchPosition.y - _startTouchPosition.y);
        currentSwipe.Normalize();
        SendFourSwipe(FindSwipeEightDirection(currentSwipe));
        SendEightSwipe(FindSwipeFourDirection(currentSwipe));
        SendEightSwipe(FindSwipeFourDirectionXType(currentSwipe));
    }

    public SwipeDirection FindSwipeEightDirection(Vector2 currentSwipe)
    {
        var direction = SwipeDirection.None;
        if (Vector2.Dot(currentSwipe, GetCardinalDirections.Up) > 0.906f)
        {
            direction = SwipeDirection.Up;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Down) > 0.906f)
        {
            direction = SwipeDirection.Down;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Left) > 0.906f)
        {
            direction = SwipeDirection.Left;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Right) > 0.906f)
        {
            direction = SwipeDirection.Right;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpRight) > 0.906f)
        {
            direction = SwipeDirection.UpRight;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpLeft) > 0.906f)
        {
            direction = SwipeDirection.UpLeft;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownLeft) > 0.906f)
        {
            direction = SwipeDirection.DownLeft;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownRight) > 0.906f)
        {
            direction = SwipeDirection.DownRight;
        }
        return direction;
    }

    public SwipeDirection FindSwipeFourDirection(Vector2 currentSwipe)
    {
        var direction = SwipeDirection.None;
        if (Vector2.Dot(currentSwipe, GetCardinalDirections.Up) > 0.707f)
        {
            direction = SwipeDirection.Up;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Down) > 0.707f)
        {
            direction = SwipeDirection.Down;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Left) > 0.707f)
        {
            direction = SwipeDirection.Left;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Right) > 0.707f)
        {
            direction = SwipeDirection.Right;
        }
        return direction;
    }

    public SwipeDirection FindSwipeFourDirectionXType(Vector2 currentSwipe)
    {
        var direction = SwipeDirection.None;
        if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpRight) > 0.707f)
        {
            direction = SwipeDirection.UpRight;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpLeft) > 0.707f)
        {
            direction = SwipeDirection.UpLeft;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownRight) > 0.707f)
        {
            direction = SwipeDirection.DownRight;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownLeft) > 0.707f)
        {
            direction = SwipeDirection.DownLeft;
        }
        return direction;
    }

    private SwipeData GetSwipeData(SwipeDirection direction)
    {
        var swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = _startTouchPosition,
            EndPosition = _endTouchPosition
        };
        return swipeData;
    }

    private void SendEightSwipe(SwipeDirection direction)
    {
        var swipeData = GetSwipeData(direction);
        OnEightSwipe?.Invoke(swipeData);
    }

    private void SendFourSwipe(SwipeDirection direction)
    {
        var swipeData = GetSwipeData(direction);
        OnFourSwipe?.Invoke(swipeData);
    }
}
