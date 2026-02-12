using UnityEngine;

public class CheckCollision : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("Destroy");
            FindFirstObjectByType<UIManager>().DecreaseScore();
        }
    }
}
