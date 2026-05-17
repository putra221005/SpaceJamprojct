using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float CameraSpeed = 2f;
    public Transform target;

    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, CameraSpeed);
    }
}
