using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Object Pool")]
    //What it is going to spawn
    [SerializeField] private GameObject objectToSpawn;

    private Transform parentPool => gameObject.transform;

    [Header("Pool Values")]
    //MaxZise and the currentSize of the pool
    private int currentSize;

    [HideInInspector] public bool autoExpand;
    [HideInInspector] public int maxSize;
    private int poolSize = 0;

    //The ObjectPool
    public Queue<GameObject> objectPool;

    private void Awake()
    {
        objectPool = new Queue<GameObject>();

        //Set values if not done correctly
        if (autoExpand) maxSize = -1;
        if (!autoExpand) poolSize = maxSize;
        if (poolSize >= maxSize && !autoExpand) poolSize = maxSize;
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
        else if (spawnedObject == null || autoExpand && spawnedObject == null)
        {
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

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(ObjectPool))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ObjectPool objPool = (ObjectPool)target;

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);

            EditorGUILayout.Space();

            objPool.autoExpand = GUILayout.Toggle(objPool.autoExpand, "AutoExpand");

            if (!objPool.autoExpand)
                objPool.maxSize = EditorGUILayout.IntField("MaxSize", objPool.maxSize);
        }
    }

#endif
    #endregion 
}
