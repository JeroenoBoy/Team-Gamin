using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Paths;
using UnityEngine;
using Util;
using NPC.UnitData;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{

    public UnitTeam Team;
    
    private ObjectPool _objPool;

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
    
    [Header("Penalty")]
    public float CurrentPenalty;
    public float PenaltyDistance = 15f;
    public float PenaltyWait;
    
    [Header("Events")]
    public UnityEvent OnWaveStart;
    
    

    #region Singleton ish
    public static readonly Dictionary<UnitTeam, SpawnManager> managers = new Dictionary<UnitTeam, SpawnManager>();
    

    private void Awake()
    {
        if (managers.ContainsKey(Team))
        {
            Debug.LogError("This manager already exists!");
            Destroy(this);
            return;
        }
        
        managers.Add(Team, this);
    }


    private void OnDestroy()
    {
        if (managers.TryGetValue(Team, out var manager) && manager == this)
            managers.Remove(Team);
    }

    #endregion


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

            while (CurrentPenalty > 0)
            {
                CurrentPenalty--;
                yield return new WaitForSeconds(PenaltyWait);
            }
            
            OnWaveStart?.Invoke();
            
            for (int i = 0; i < 10; i++)
            {
                var obj = _objPool.SpawnObject();
                if (!obj) break;
                
                obj.transform.position = transform.position + Random.insideUnitSphere.With(y : 0) * multiplier;
                SetValues(obj);
                yield return new WaitForSeconds(TimeBetweenSpawn);
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
        go.targetCastle = TargetCastle;
        go.state = behaviourMenu.unitState;

        go.path = go.state switch
        {
            UnitState.GuardPathA => guardPath1,
            UnitState.GuardPathB => guardPath2,
            _ => paths[behaviourMenu.pathIndex]
        };
        
        go.Bind();
    }
}
