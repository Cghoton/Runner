using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private float DistanceFromPlayer = 40f;

    [SerializeField]
    private Transform playerTrans;

    [SerializeField]
    private float TimeToSpawn = 3f;

    [SerializeField]
    private GameObject[] dangerPrefabs;

    [SerializeField]
    private GameObject[] surroundingsPrefabs;

    [SerializeField]
    private Vector3 offset;

    

    private bool canSpawn = true;
    void Start()
    {
        StartCoroutine(SpawnObj());
        StartCoroutine(SpawnSurroundings());
    }

    void Update()
    {
        transform.position = new Vector3(playerTrans.position.x + DistanceFromPlayer, transform.position.y, transform.position.z);
    }

    private IEnumerator SpawnObj()
    {
        while (canSpawn)
        {
            Instantiate(dangerPrefabs[Random.Range(0, dangerPrefabs.Length)], transform.position + offset * Random.Range(0, 3), transform.rotation);

            yield return new WaitForSeconds(TimeToSpawn);
        }
        
    }
    private IEnumerator SpawnSurroundings()
    {
        while (canSpawn)
        {
            Instantiate(surroundingsPrefabs[Random.Range(0, dangerPrefabs.Length)], new Vector3(transform.position.x, transform.position.y, 1.2f), transform.rotation);

            yield return new WaitForSeconds(TimeToSpawn);
        }
    }
}
