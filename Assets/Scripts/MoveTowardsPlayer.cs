using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    float speed;
    public Vector3 destroyLocation;

    private void Start()
    {
        speed = 1.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z <= destroyLocation.z) 
        { 
            Destroy(gameObject);
        }
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}
