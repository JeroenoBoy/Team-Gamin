using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Object Pool")]
    //What it is going to spawn
    [SerializeField] protected GameObject objectToSpawn;

    //Spawn Value's
    private Transform spawnPos => gameObject.transform;
    private protected Transform parentPool => gameObject.transform;

    [Header("Pool Values")]
    //MaxZise and the currentSize of the pool
    [SerializeField] protected int currentSize;
    [SerializeField] protected int poolSize;
    [SerializeField] protected int maxSize;

    //The ObjectPool
    public Queue<GameObject> objectPool;

    // Set this to true if you want to expand the pool if you run out of pooled objects.
    [SerializeField] private bool autoExpand = false;
    // The amount of new objects added when the pool runs out of objects.
    [SerializeField] private int expansionSize = 1;

    private void Awake()
    {
        objectPool = new Queue<GameObject>();
    }

    /// <summary>
    /// Manages the object pool : Spawns the object with the right parameters and reuse's it 
    /// </summary>
    public virtual GameObject SpawnObject(GameObject currentObject = null)
    {
        //Check if the object is null and set the object
        if (currentObject == null)
        {
            currentObject = objectToSpawn;
        }

        GameObject spawnedObject = GetPooledObject();

        if (poolSize == maxSize && currentSize == poolSize)
        {
            if (spawnedObject == null)
                return null;
            else
            {
                spawnedObject.transform.position = transform.position;
                spawnedObject.transform.rotation = Quaternion.identity;
            }
        }
        else if (currentSize <= poolSize && spawnedObject == null || autoExpand && spawnedObject == null)
        {
            if (autoExpand && poolSize != maxSize)
                poolSize += expansionSize;

            spawnedObject = Instantiate(currentObject, transform.position, Quaternion.identity, parentPool);
            spawnedObject.name = currentObject.name + "_" + currentSize;
            currentSize++;
        }
        else
        {
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = Quaternion.identity;
        }

        objectPool.Enqueue(spawnedObject);
        spawnedObject.SetActive(true);
        return spawnedObject; 
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return transform.GetChild(i).gameObject;
            }
        }

        return null;
    }
}
