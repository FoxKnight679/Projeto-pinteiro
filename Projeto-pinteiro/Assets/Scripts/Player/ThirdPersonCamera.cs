using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // O personagem
    public Vector3 offset = new Vector3(0, 3, -6);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
