using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public Vector3 spawnLocation;


    public GameObject[] wallPiece;

    float wallPieceWidth;

    int height, width; //no. of blocks across and down
    int curr;
    void Start()
    {
        float zDistance = Mathf.Abs(
    transform.position.z - Camera.main.transform.position.z
);


        /*        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(
                    new Vector3(Screen.width, Screen.height, zDistance)
                );


        */

        Vector3 screenBounds = new Vector3(6, 6, 0);
        GameObject wallPieceInstance = Instantiate(wallPiece[0]);
        wallPieceWidth = wallPieceInstance.GetComponent<Collider>().bounds.size.x;
        Destroy(wallPieceInstance);
        
        Debug.Log(wallPieceWidth);

        height = (int)(screenBounds.y / wallPieceWidth + 2*wallPieceWidth);
        width = (int)(screenBounds.x / wallPieceWidth + 2 * wallPieceWidth);

        curr = 0;

    }

    public void StartGame()
    {
        Random.InitState(12345);
        curr = 0;
        StartCoroutine(SpawnWall());
    }


    public void StopGame()
    {
        StopAllCoroutines();
    }

    void CreateWall()
    {

        GameObject parentWall = new GameObject();
        parentWall.transform.position = spawnLocation;

        int skipH = Random.Range(2, height - 2);
        int skipW = Random.Range(2, width - 2);

        float upperLimit = (height * wallPieceWidth)/2.0f;
        float leftLimit = (width * wallPieceWidth)/2.0f;

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                Vector3 loc = new Vector3(leftLimit, upperLimit, parentWall.transform.position.z);

                if (!(i == skipH && j == skipW))
                {
                    Instantiate(wallPiece[curr], loc, Quaternion.Euler(0, 0, 0), parentWall.transform);
                }
                else
                {
                    GameObject wall = new GameObject("Hole");
                    wall.transform.SetParent(parentWall.transform);
                    wall.transform.SetPositionAndRotation(loc, Quaternion.identity);
                    wall.AddComponent<BoxCollider>();
                    wall.GetComponent<BoxCollider>().isTrigger = true;
                    wall.AddComponent<HoleTrigger>();
                }
                leftLimit += wallPieceWidth;

                Debug.Log(leftLimit);
            }
            leftLimit = (width * wallPieceWidth)/2.0f;
            upperLimit += wallPieceWidth;
        }

        parentWall.transform.position = new Vector3(spawnLocation.x - width * wallPieceWidth, spawnLocation.y - height * wallPieceWidth, spawnLocation.z);
        parentWall.AddComponent<MoveTowardsPlayer>();

        curr = (curr + 1) % wallPiece.Length;
    }
    
    IEnumerator SpawnWall()
    {
        while (true)
        {
            CreateWall();
            yield return new WaitForSeconds(Random.Range(7.0f, 10.0f));
        }
    }
}
