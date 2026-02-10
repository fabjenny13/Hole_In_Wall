using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    float speed;
    public Vector3 playerLocation;

    private void Start()
    {
        playerLocation = FindFirstObjectByType<PlayerMovement>().gameObject.transform.position;
        speed = 1.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z <= playerLocation.z - 1) 
        { 
            Destroy(gameObject);
        }
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
