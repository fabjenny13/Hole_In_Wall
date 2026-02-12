using System;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    bool hasCrashed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WallPiece" && !hasCrashed)
        {
            hasCrashed = true;
            other.GetComponent<Animator>().SetTrigger("Destroy");
            FindFirstObjectByType<UIManager>().DecreaseScore();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasCrashed = false;
    }
}
