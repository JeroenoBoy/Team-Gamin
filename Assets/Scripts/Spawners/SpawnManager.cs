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
    private ObjectPool _objPool;

    public UnitTeam Team;

    public Traits allTraits;

    public StatPoints statPoints;
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

    /// <summary>
    /// Spawn the agents with the objectPool
    /// </summary>
    /// <returns></returns>
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
                var obj = _objPool.SpawnObject(); //Use the objectPool
                if (!obj) break;

                obj.transform.position = transform.position + Random.insideUnitSphere.With(y: 0) * multiplier;
                SetValues(obj); //Give the agent his traits
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
        int t = Random.Range(1, 3);

        //Set all the base values
        go.attackDamage = (int)statPoints.data[0].value;
        go.attackSpeed = statPoints.data[1].value;
        go.movementSpeed = statPoints.data[2].value;
        go.sightRange = (int)statPoints.data[3].value;
        go.defense = (int)statPoints.data[4].value;

        string traits = "";

        while (t > 0) //Add all the extra traits 
        {
            int a = Random.Range(0, allTraits.TraitsClass.Length);

            go.attackDamage += allTraits.TraitsClass[a].atkdmg;
            go.attackSpeed += allTraits.TraitsClass[a].atkspd;
            go.movementSpeed += allTraits.TraitsClass[a].movspd;
            go.sightRange += allTraits.TraitsClass[a].sightRange;
            go.defense += allTraits.TraitsClass[a].defense;

            traits = string.Format(traits + " " + allTraits.TraitsClass[a].name);

            // --- Rip 2/17/2022 --- //
            //go.name = string.Format(go.name + " [" + allTraits.TraitsClass[a].name + "]");
            t--;
        }

        go.traits = traits.ToString();
        go.targetCastle = TargetCastle;
        go.state = behaviourMenu.unitState;
    }
}
