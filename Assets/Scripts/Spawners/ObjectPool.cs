using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Spawners
{
    public class ObjectPool : MonoBehaviour
    {
        [Header("Object Pool")]
        //What it is going to spawn
        [SerializeField] private GameObject _objectToSpawn;
        [SerializeField] private Transform  _parentPool;

        [HideInInspector] public bool AutoExpand;
        [HideInInspector] public int MaxSize;
    
        //  Max sise and the currentSize of the pool
        private int _currentSize;
        private int _poolSize = 0;

        //  The ObjectPool
        public Queue<GameObject> Items;

    
        private void Awake()
        {
            Items = new Queue<GameObject>();

            //  Set values if not done correctly
            if (AutoExpand) MaxSize = -1;
            if (!AutoExpand) _poolSize = MaxSize;
            if (_poolSize >= MaxSize && !AutoExpand) _poolSize = MaxSize;
        }

    
        /// <summary>
        /// Manages the object pool : Spawns the object with the right parameters and reuse's it 
        /// </summary>
        public virtual GameObject SpawnObject(GameObject currentObject = null)
        {
            //  Check if the object is null and set the object
            if (currentObject == null)
            {
                currentObject = _objectToSpawn;
            }

            GameObject spawnedObject = GetPooledObject(); // Get inactive objects

            if (_poolSize == MaxSize && _currentSize == _poolSize)
            {
                if (spawnedObject == null)
                    return null;
            
                spawnedObject.transform.position = _parentPool.transform.position;
                spawnedObject.transform.rotation = Quaternion.identity;
            }
            else if (spawnedObject == null || AutoExpand && spawnedObject == null) // Spawn a new object
            {
                spawnedObject = Instantiate(currentObject, _parentPool.transform.position, Quaternion.identity, _parentPool);
                spawnedObject.name = currentObject.name + "_" + _currentSize;
                _currentSize++;
            }
            else
            {   //  Reuse the object
                spawnedObject.transform.position = _parentPool.transform.position;
                spawnedObject.transform.rotation = Quaternion.identity;
            }

            Items.Enqueue(spawnedObject);
            spawnedObject.SetActive(true);
            return spawnedObject;
        }

    
        /// <summary>
        /// Get all the children of the objectPool and return the object that is inactive
        /// </summary>
        /// <returns></returns>
        private GameObject GetPooledObject()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.activeInHierarchy) //Check if its not active
                {
                    return transform.GetChild(i).gameObject; //Return object
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

                objPool.AutoExpand = GUILayout.Toggle(objPool.AutoExpand, "AutoExpand");

                if (!objPool.AutoExpand)
                    objPool.MaxSize = EditorGUILayout.IntField("MaxSize", objPool.MaxSize);
            }
        }

#endif
        #endregion 
    }
}
