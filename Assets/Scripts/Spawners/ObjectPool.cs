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
    private bool hasInactiveChild = false;

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

        GameObject spawnedObject = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeInHierarchy)
            {
                hasInactiveChild = true;
                currentObject = transform.GetChild(i).gameObject;
            }
        }

        if (currentSize < poolSize && !hasInactiveChild || autoExpand && !hasInactiveChild)
        {
            if (autoExpand)
                poolSize += expansionSize;

            spawnedObject = Instantiate(currentObject, transform.position, Quaternion.identity);
            spawnedObject.gameObject.transform.parent = parentPool;
            spawnedObject.name = currentObject.name + "_" + currentSize;
            currentSize++;
            hasInactiveChild = false;
        }
        else
        {
            //Remove the object from the queue and if its null set the values and spawn the object
            spawnedObject = objectPool.Dequeue();
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(currentObject, transform.position, Quaternion.identity);
                spawnedObject.name = currentObject.name + "_" + currentSize;
                currentSize++;
            }
            else
            {
                Debug.Log(spawnedObject.name);
                spawnedObject.transform.position = transform.position;
                spawnedObject.transform.rotation = Quaternion.identity;
            }
        }
        hasInactiveChild = false;
        objectPool.Enqueue(spawnedObject);
        spawnedObject.SetActive(true);
        return spawnedObject;
    }
}
