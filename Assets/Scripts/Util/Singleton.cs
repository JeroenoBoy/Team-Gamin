using UnityEngine;

namespace Util
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }


        protected virtual void OnEnable()
        {
            if (!Instance || Instance == this) Instance = this as T;
            else
            {
                Destroy(this);
                Debug.LogError("Instance already exists!");
            }
        }


        protected virtual void OnDisable()
        {
            if(Instance == this) Instance = null;
        }
    }
}