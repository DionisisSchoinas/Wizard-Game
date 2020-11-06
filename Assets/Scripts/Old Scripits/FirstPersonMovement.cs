using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    Vector3 velocity;

    void FixedUpdate()
    {
        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        velocity.y = UpDown() * speed * Time.deltaTime;
        velocity.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(velocity.x, velocity.y, velocity.z);
    }

    float UpDown()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            return -1f;
        }
        return 0f;
    }
}
