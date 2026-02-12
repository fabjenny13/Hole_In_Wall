using System.Collections;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public Vector3 spawnLocation;


    public GameObject[] wallPiece;

    int height, width; //no. of blocks across and down
    int curr;
    void Start()
    {
        height = 4;
        width = 8;

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

        int skipH = Random.Range(1, height - 1);
        int skipW = Random.Range(1, width - 1);

        int upperLimit = height / 2;
        int leftLimit = width / 2;

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
                leftLimit++;
            }
            leftLimit = width / 2;
            upperLimit++;
        }

        parentWall.transform.position = new Vector3(spawnLocation.x - width, spawnLocation.y - height, spawnLocation.z);
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
