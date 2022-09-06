using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleJoy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform handler;
    [SerializeField] private RectTransform circle;
    [SerializeField] private bool softRelease;
    [SerializeField] private float releaseSpeed = 8;

    private State state;

    private Vector2 startPosition;
    private Vector2 direction;
    private float maxValue;

    public event UnityAction<Vector2> onJoysickMove;

    private void Start()
    {
        state = State.waiting;
        startPosition = handler.position;
        maxValue = circle.sizeDelta.x / 2;
        circle.position = handler.position;
    }

    private void Update()
    {
        switch (state)
        {
            case State.moving:
                onJoysickMove?.Invoke(direction);
                break;

            case State.stoping:
                onJoysickMove?.Invoke(Vector2.zero);
                state = State.waiting;
                break;

            case State.releasing:
                if (direction.magnitude > 0.01f)
                {
                    direction = Vector2.Lerp(direction, Vector2.zero, releaseSpeed * Time.deltaTime);
                    onJoysickMove?.Invoke(direction);
                }
                else
                {
                    onJoysickMove?.Invoke(Vector2.zero);
                    state = State.stoping;
                }
                break;

            case State.waiting:
                break;

            default:
                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        state = State.moving;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var delta = eventData.position - startPosition;
        if (delta.magnitude < maxValue)
            handler.position = eventData.position;
        else
        {
            delta = Vector3.ClampMagnitude(delta, maxValue);
            handler.position = startPosition + delta;
        }

        direction = 1 / maxValue * delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handler.position = startPosition;
        if (softRelease)
            state = State.releasing;
        else
            state = State.stoping;
    }

    private enum State {moving, stoping, releasing, waiting }
}
