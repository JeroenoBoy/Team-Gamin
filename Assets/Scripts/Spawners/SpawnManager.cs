using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public ObjectPool objPool;

    [Range(0.1f, 2f)]
    public float timeBetweenSpawn;

    private void Start()
    {
        objPool = GetComponent<ObjectPool>();
        StartCoroutine(WaveSpawner());
    }

    protected virtual IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawn);

            var obj = objPool.SpawnObject();
        }
    }
}
