using UnityEngine;

public class ConstrainToCamera : MonoBehaviour
{
    private Vector3 screenBounds;
    private float objectWidth;
    private float objectHeight;
    float zDistance;

    void Start()
    {
        zDistance = Mathf.Abs(
            transform.position.z - Camera.main.transform.position.z
        );

        screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, zDistance)
        );
    }


    void LateUpdate()
    {
        Vector3 viewPos = transform.position;

        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);

        transform.position = viewPos;
    }
}
