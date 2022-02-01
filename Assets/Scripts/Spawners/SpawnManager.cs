using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Range(0.1f, 2f)]
    public float timeBetweenSpawn;

    public Transform objToSpawn;

    private void Start()
    {
        StartCoroutine(WaveSpawner());
    }

    protected virtual IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawn);

            Instantiate(objToSpawn, transform.position, Quaternion.identity);
        }
    }
}
