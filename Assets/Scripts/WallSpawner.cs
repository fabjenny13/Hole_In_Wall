using System.Collections;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public Vector3 spawnLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnWall());
    }

    // Update is called once per frame
    void Update()
    {
        
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
