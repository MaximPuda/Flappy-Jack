using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    [Header("Deadzone")]
    [SerializeField] private bool deadZone;
    [SerializeField, Min(0)] private float deadX;
    [SerializeField, Min(0)] private float deadY;
    [SerializeField, Min(0)] private float deadZ;
    
    [Header("Smoothing")]
    [SerializeField] private bool smoothing;
    [SerializeField] private float rate;
    
    [Header("Offset")]
    [SerializeField] private Vector3 offset;

    private Vector3 newPos =  new Vector3();
    private Vector3 targetPos;

    private void Update()
    {
        targetPos = target.position;

        if (deadZone)
        {
            var delta = target.position - transform.position + offset;

            if (Mathf.Abs(delta.x) > deadX 
                || Mathf.Abs(delta.y) > deadY
                || Mathf.Abs(delta.z) > deadZ)
            {
                if (delta.x < - deadX)
                    newPos.x = targetPos.x + deadX + offset.x;
                else if (delta.x > deadX)
                    newPos.x = targetPos.x - deadX + offset.x;

                if (delta.y < -deadY)
                    newPos.y = targetPos.y + deadY + offset.y;
                else if (delta.y > deadY)
                    newPos.y = targetPos.y - deadY + offset.y;

                if (delta.z < -deadZ)
                    newPos.z = targetPos.z + deadZ + offset.z;
                else if (delta.z > deadZ)
                    newPos.z = targetPos.z - deadZ + offset.z;
            }
        }
        else
             newPos = targetPos + offset;

        if (smoothing && Vector3.Magnitude(newPos - transform.position) > 0.1f)
        {
            var tempPos = Vector3.Lerp(transform.position, newPos, rate * Time.deltaTime);
            transform.position = tempPos;
        }
        else
            transform.position = newPos;
    }

    private void OnDrawGizmosSelected()
    {
        if (deadZone)
        {
            var gizmoPos = transform.position - offset;
            var gizmoSize = new Vector3(deadX, deadY, deadZ) * 2;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(gizmoPos, gizmoSize);
        }
    }
}
