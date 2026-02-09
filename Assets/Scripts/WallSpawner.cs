using System.Collections;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public Vector3 spawnLocation;


    public GameObject wallPiece;
    int height, width; //no. of blocks across and down
    void Start()
    {
        //StartCoroutine(SpawnWall());
        height = 9;
        width = 9;

        CreateWall();
    }

    void CreateWall()
    {

        GameObject parentWall = new GameObject();
        parentWall.transform.position = spawnLocation;

        int skipH = Random.Range(1, height - 2);
        int skipW = Random.Range(1, width - 2);

        int upperLimit = height / 2;
        int leftLimit = width / 2;

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if (!(i == skipH && j == skipW))
                {
                    Vector3 loc = new Vector3(leftLimit, upperLimit, parentWall.transform.position.z);
                    Instantiate(wallPiece, loc, Quaternion.Euler(0, 0, 0), parentWall.transform);
                }
                leftLimit++;
            }
            leftLimit = width / 2;
            upperLimit++;
        }
    }
    
    IEnumerator SpawnWall()
    {
        while (true)
        {
            Instantiate(wallPrefab, spawnLocation, Quaternion.Euler(0,0,0));
            yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
        }
    }
}
