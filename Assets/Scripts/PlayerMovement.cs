using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float inputX, inputY;
    float speed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector2(inputX, inputY) * speed * Time.deltaTime);
        
    }
}
