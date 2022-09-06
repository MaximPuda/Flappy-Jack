using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MyJoystick : MonoBehaviour
{
    [SerializeField] private Image handler;
    [SerializeField] private Image circle;

    public static UnityAction<Vector2> onJoystickMove;
    private Vector2 direction = new Vector3();
    private Vector2 startSwipe = new Vector2();

    private float maxValue;

    private void Start()
    {
        maxValue = circle.rectTransform.sizeDelta.x / 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            JoystickView(true);
            startSwipe = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            circle.transform.position = startSwipe;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousePos = Input.mousePosition;
            var delta = mousePos - startSwipe;
            if (delta.magnitude < maxValue)
                handler.transform.position = mousePos;
            else
            {
                delta = Vector3.ClampMagnitude(delta, maxValue);
                handler.transform.position = startSwipe + delta;
            }

            direction = 1 / maxValue * delta;
            onJoystickMove?.Invoke(direction);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            onJoystickMove?.Invoke(Vector2.zero);
            JoystickView(false);
        }
    }

    private void JoystickView(bool enabled)
    {
        circle.enabled = enabled;
        handler.enabled = enabled;
    }
}
