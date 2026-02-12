using UnityEngine;

public class HoleTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FindFirstObjectByType<UIManager>().IncreaseScore();
        }
    }
}
