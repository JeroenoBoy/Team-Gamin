using System.Collections;
using System.Collections.Generic;
using Controllers.Paths;
using UnityEngine;
using Util;
using NPC.UnitData;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    private ObjectPool _objPool;

    public Traits allTraits;

    public newStatPoints statPoints;
    public BehaviourMenu behaviourMenu;

    [Range(0.1f, 1f)]
    public float TimeBetweenSpawn;

    [Range(0.1f, 20f)]
    public float TimeBetweenWave;

    [Range(1f, 20f)]
    public float multiplier;

    public Transform TargetCastle;

    [Header("Paths")]
    public PathController guardPath1;
    public PathController guardPath2;
    public PathController[] paths;

    [Header("Events")]
    public UnityEvent OnWaveStart;

    private void Start()
    {
        _objPool = GetComponent<ObjectPool>();
        StartCoroutine(WaveSpawner());
    }

    protected virtual IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenWave);
            OnWaveStart?.Invoke();

            for (int i = 0; i < 10; i++)
            {
                var obj = _objPool.SpawnObject();
                if (!obj) break;

                obj.transform.position = transform.position + Random.insideUnitSphere.With(y: 0) * multiplier;
                SetValues(obj);
                yield return new WaitForSeconds(TimeBetweenSpawn);
            }
        }
    }

    /// <summary>
    /// Set all the values the agent needs
    /// </summary>
    /// <param name="obj"></param>
    private void SetValues(GameObject obj)
    {
        var go = obj.GetComponent<UnitSettings>();

        GetRandomTrait(go);

        go.path = go.state switch
        {
            UnitState.GuardPathA => guardPath1,
            UnitState.GuardPathB => guardPath2,
            _ => paths[behaviourMenu.pathIndex]
        };

        go.Bind();
    }

    /// <summary>
    /// Get random trait and add them to the current stats
    /// </summary>
    /// <param name="go"></param>
    private void GetRandomTrait(UnitSettings go)
    {
        var i = Random.Range(0, allTraits.TraitsClass.Length);

        go.attackDamage = (int)statPoints.data[0].value + allTraits.TraitsClass[i].atkdmg;
        go.attackSpeed = statPoints.data[1].value + allTraits.TraitsClass[i].atkspd;
        go.movementSpeed = statPoints.data[2].value + allTraits.TraitsClass[i].movspd;
        go.sightRange = (int)statPoints.data[3].value + allTraits.TraitsClass[i].sightRange;
        go.defense = (int)statPoints.data[4].value + allTraits.TraitsClass[i].defense;
        go.targetCastle = TargetCastle;
        go.state = behaviourMenu.unitState;
        go.name = string.Format(go.name + " [" + allTraits.TraitsClass[i].name + "]") ;
    }
}
