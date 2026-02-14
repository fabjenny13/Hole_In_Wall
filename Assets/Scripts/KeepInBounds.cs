using UnityEngine;

public class ConstrainToCamera : MonoBehaviour
{
    private Vector3 screenBounds;
    float zDistance;

    void Start()
    {
        zDistance = Mathf.Abs(
            transform.position.z - Camera.main.transform.position.z
        );

        screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, zDistance)
        );

        Debug.Log(screenBounds);
    }


    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + Camera.main.transform.position.x, screenBounds.x + Camera.main.transform.position.x);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + Camera.main.transform.position.y, screenBounds.y + Camera.main.transform.position.y);

        transform.position = viewPos;
    }
}
