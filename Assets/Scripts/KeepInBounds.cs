using UnityEngine;

public class ConstrainToCamera : MonoBehaviour
{
    public Vector3 screenBounds;
    float zDistance;

    void Start()
    {
/*        zDistance = Mathf.Abs(
            transform.position.z - Camera.main.transform.position.z
        );
*/
        /*screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, zDistance)
        );*/

        
    }


    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x, screenBounds.x);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y, screenBounds.y);

        transform.position = viewPos;
    }
}
