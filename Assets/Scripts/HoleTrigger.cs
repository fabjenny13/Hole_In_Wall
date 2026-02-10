using UnityEngine;

public class HoleTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindFirstObjectByType<UIManager>().IncreaseScore();
        }
    }
}
