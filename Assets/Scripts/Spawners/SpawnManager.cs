using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private ObjectPool _objPool;

    [Range(0.1f, 2f)]
    public float TimeBetweenSpawn;

    private void Start()
    {
        _objPool = GetComponent<ObjectPool>();
        StartCoroutine(WaveSpawner());
    }

    protected virtual IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenSpawn);

            var obj = _objPool.SpawnObject();
        }
    }
}
