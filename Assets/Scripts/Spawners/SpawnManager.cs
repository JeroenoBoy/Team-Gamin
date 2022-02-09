using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class SpawnManager : MonoBehaviour
{
    private ObjectPool _objPool;

    [Range(0.1f, 1f)]
    public float TimeBetweenSpawn;

    [Range(0.1f, 20f)]
    public float TimeBetweenWave;

    private int i;

    private void Start()
    {
        _objPool = GetComponent<ObjectPool>();
        StartCoroutine(WaveSpawner());
    }

    protected virtual IEnumerator WaveSpawner()
    {
        while (i < 10)
        {
            yield return new WaitForSeconds(TimeBetweenSpawn);
            
            i++;
            var obj = _objPool.SpawnObject();
            obj.transform.position = Random.insideUnitSphere.With(y : 0);

            if (i == 10)
            {
                yield return new WaitForSeconds(TimeBetweenWave);
                i = 0;
            }
        }
    }
}
