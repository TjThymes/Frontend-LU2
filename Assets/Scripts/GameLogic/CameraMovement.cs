using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.y -= 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.x += 1;
        }

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
