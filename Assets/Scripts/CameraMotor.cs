using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.15f;
    public float boundY = 0.05f;
    public float smoothSpeed = 10f;

    private void FixedUpdate()
    {
        Vector3 delta = Vector3.zero;

        // X sınırlamasının içinde mi diye kontrol eder
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }
        
        // Y sınırlamasının içinde mi diye kontrol eder
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        Vector3 desiredPosition = transform.position + delta;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
