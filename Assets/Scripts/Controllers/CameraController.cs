using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed;
    public float scrollSpeed;
    public float minY;
    public float maxY;
    public Vector2 panLimit;

    void Update()
    {
        Vector3 pos = transform.position;
        //controls the camera with WASD controls
        if(Input.GetKey(KeyCode.W))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
