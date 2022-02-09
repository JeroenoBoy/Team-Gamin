using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using NPC.UnitData;

public class SpawnManager : MonoBehaviour
{
    private ObjectPool _objPool;

    public newStatPoints statPoints;

    [Range(0.1f, 1f)]
    public float TimeBetweenSpawn;

    [Range(0.1f, 20f)]
    public float TimeBetweenWave;

    [Range(1f, 20f)]
    public float multiplier;

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
            obj.transform.position = transform.position + Random.insideUnitSphere.With(y : 0) * multiplier;
            SetValues(obj);

            if (i == 10)
            {
                yield return new WaitForSeconds(TimeBetweenWave);
                i = 0;
            }
        }
    }

    private void SetValues(GameObject obj)
    {
        var go = obj.GetComponent<UnitSettings>();

        go.attackDamage = (int)statPoints.data[0].value;
        go.attackSpeed = statPoints.data[1].value;
        go.movementSpeed = statPoints.data[2].value;
        go.sightRange = (int)statPoints.data[3].value;
        go.defense = (int)statPoints.data[4].value;
    }
}
